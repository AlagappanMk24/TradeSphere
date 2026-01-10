using TradeSphere.Domain.Entities;
using TradeSphere.Infrastructure.Data.DataSeed.DTOs;

namespace TradeSphere.Infrastructure.Data.DataSeed
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(this TradeSphereDbContext context, string contentRootPath)
        {
            var option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var seedFilesPath = Path.Combine(contentRootPath, "Data", "DataSeed", "SeedFiles");
            if (!Directory.Exists(seedFilesPath))
            {
                seedFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "TradeSphere.Infrastructure", "Data", "DataSeed", "SeedFiles");
            }
            if (!Directory.Exists(seedFilesPath))
            {
                throw new DirectoryNotFoundException($"Seed files not found at: {seedFilesPath}");
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Read Roles From JSON
                if (!context.Roles.Any())
                {
                    var rolesData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "AppRoles.json"));
                    var roles = JsonSerializer.Deserialize<List<ApplicationRole>>(rolesData, option)!;
                    context.Roles.AddRange(roles);
                    await context.SaveChangesAsync();
                }

                // Add Users
                if (!context.Users.Any())
                {
                    var userData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "AppUsers.json"));
                    var users = JsonSerializer.Deserialize<List<ApplicationUser>>(userData, option)!;
                    foreach (var user in users)
                    {
                        user.Id = 0;
                        user.NormalizedUserName = user.UserName.ToUpper();
                        user.NormalizedEmail = user.Email.ToUpper();
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        user.ConcurrencyStamp = Guid.NewGuid().ToString();
                        user.PasswordHash = passwordHasher.HashPassword(user, "P@ssw0rd");
                    }
                    context.Users.AddRange(users);
                    await context.SaveChangesAsync();
                }

                // AddUserROle
                if (!context.UserRoles.Any())
                {
                    var userRolesData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "UserRoles.json"));
                    var userRolesMappings = JsonSerializer.Deserialize<List<UserRoleMapping>>(userRolesData, option)!;

                    var userRoles = new List<IdentityUserRole<int>>();

                    foreach (var mapping in userRolesMappings)
                    {
                        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == mapping.UserEmail);
                        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == mapping.RoleName);

                        if (user != null && role != null)
                        {
                            userRoles.Add(new IdentityUserRole<int>
                            {
                                UserId = user.Id,
                                RoleId = role.Id
                            });
                        }
                    }

                    context.UserRoles.AddRange(userRoles);
                    await context.SaveChangesAsync();
                }

                // Add Category
                if (!context.Categories.Any())
                {
                    var categoryData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "Categories.json"));
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoryData, option)!;
                    context.Categories.AddRange(categories);
                    await context.SaveChangesAsync();
                }

                // Add Product
                if (!context.Products.Any())
                {
                    var productData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "Products.json"));

                    var products = JsonSerializer.Deserialize<List<Product>>(productData, option)!;

                    // Load categories once (performance + clarity)
                    var categories = await context.Categories.ToListAsync();

                    foreach (var product in products)
                    {
                        var category = categories.FirstOrDefault(c =>
                            c.Name.Equals(product.CategoryName, StringComparison.OrdinalIgnoreCase)) ?? throw new Exception($"Category '{product.CategoryName}' not found for product '{product.Name}'");
                        
                        product.CategoryId = category.Id;

                        // Optional but recommended
                        product.CategoryName = null;
                    }
                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }

                // Add ShoppingCarts
                if (!context.ShoppingCarts.Any())
                {
                    var shoppingCartData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "ShoppingCarts.json"));

                    var cartSeeds = JsonSerializer.Deserialize<List<ShoppingCartSeedDto>>(shoppingCartData, option)!;

                    var users = await context.Users.ToListAsync();

                    var shoppingCarts = new List<ShoppingCart>();

                    foreach (var seed in cartSeeds)
                    {
                        var user = users.FirstOrDefault(u =>
                            u.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase)) ?? throw new Exception($"User '{seed.UserEmail}' not found for ShoppingCart");
                        
                        shoppingCarts.Add(new ShoppingCart
                        {
                            ApplicationUserId = user.Id
                        });
                    }
                    context.ShoppingCarts.AddRange(shoppingCarts);
                    await context.SaveChangesAsync();
                }

                // Add CartItems
                if (!context.CartItems.Any())
                {
                    var cartItemData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "CartItems.json"));
                    var cartItemSeeds = JsonSerializer.Deserialize<List<CartItemSeedDto>>(cartItemData, option)!;

                    var products = await context.Products.ToListAsync();
                    var users = await context.Users
                        .Include(u => u.ShoppingCart)
                        .ToListAsync();

                    var cartItems = new List<CartItem>();

                    foreach (var seed in cartItemSeeds)
                    {
                        var user = users.FirstOrDefault(u =>
                            u.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"User '{seed.UserEmail}' not found");

                        var product = products.FirstOrDefault(p =>
                            p.Name.Equals(seed.ProductName, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"Product '{seed.ProductName}' not found");

                        cartItems.Add(new CartItem
                        {
                            Quantity = seed.Quantity,
                            ShoppingCartId = user.ShoppingCart.Id,
                            ProductId = product.Id
                        });
                    }
                    context.CartItems.AddRange(cartItems);
                    await context.SaveChangesAsync();
                }

                // Add Orders
                if (!context.Orders.Any())
                {
                    var ordersData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "Orders.json"));

                    var orderSeeds = JsonSerializer.Deserialize<List<OrderSeedDto>>(ordersData, option)!;

                    var users = await context.Users.ToListAsync();
                    var orders = new List<Order>();

                    foreach (var seed in orderSeeds)
                    {
                        var user = users.FirstOrDefault(u =>
                            u.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"User '{seed.UserEmail}' not found for Order");

                        orders.Add(new Order
                        {
                            OrderDate = seed.OrderDate,
                            TotalAmount = seed.TotalAmount,
                            Status = seed.Status,
                            ApplicationUserId = user.Id
                        });
                    }

                    context.Orders.AddRange(orders);
                    await context.SaveChangesAsync();
                }

                // Add OrderItems
                if (!context.OrderItems.Any())
                {
                    var orderItemsData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "OrderItems.json"));

                    var orderItemSeeds = JsonSerializer.Deserialize<List<OrderItemSeedDto>>(orderItemsData, option)!;

                    var orders = await context.Orders
                        .Include(o => o.ApplicationUser)
                        .ToListAsync();

                    var products = await context.Products.ToListAsync();

                    var orderItems = new List<OrderItem>();

                    foreach (var seed in orderItemSeeds)
                    {
                        var order = orders.FirstOrDefault(o =>
                            o.ApplicationUser.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"Order not found for user '{seed.UserEmail}'");

                        var product = products.FirstOrDefault(p =>
                            p.Name.Equals(seed.ProductName, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"Product '{seed.ProductName}' not found");

                        orderItems.Add(new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = seed.Quantity,
                            UnitPrice = seed.UnitPrice
                        });
                    }

                    context.OrderItems.AddRange(orderItems);
                    await context.SaveChangesAsync();
                }

                // Add FeedBacks
                if (!context.FeedBacks.Any())
                {
                    var feedBacksData = await File.ReadAllTextAsync(
                        Path.Combine(seedFilesPath, "FeedBacks.json"));

                    var feedBackSeeds =
                        JsonSerializer.Deserialize<List<FeedBackSeedDto>>(feedBacksData, option)!;

                    var users = await context.Users.ToListAsync();
                    var products = await context.Products.ToListAsync();

                    var feedBacks = new List<FeedBack>();

                    foreach (var seed in feedBackSeeds)
                    {
                        var user = users.FirstOrDefault(u =>
                            u.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"User '{seed.UserEmail}' not found for Feedback");

                        var product = products.FirstOrDefault(p =>
                            p.Name.Equals(seed.ProductName, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"Product '{seed.ProductName}' not found for Feedback");

                        feedBacks.Add(new FeedBack
                        {
                            ApplicationUserId = user.Id,
                            ProductId = product.Id,
                            Rating = seed.Rating,
                            Comment = seed.Comment,
                            CreatedAt = seed.CreatedAt
                        });
                    }

                    context.FeedBacks.AddRange(feedBacks);
                    await context.SaveChangesAsync();
                }

                // Add RefreshTokens
                if (!context.RefreshTokens.Any())
                {
                    var refreshTokensData = await File.ReadAllTextAsync(
                        Path.Combine(seedFilesPath, "RefreshTokens.json"));

                    var tokenSeeds =
                        JsonSerializer.Deserialize<List<RefreshTokenSeedDto>>(refreshTokensData, option)!;

                    var users = await context.Users.ToListAsync();

                    var refreshTokens = new List<RefreshToken>();

                    foreach (var seed in tokenSeeds)
                    {
                        var user = users.FirstOrDefault(u =>
                            u.Email.Equals(seed.UserEmail, StringComparison.OrdinalIgnoreCase))
                            ?? throw new Exception($"User '{seed.UserEmail}' not found for RefreshToken");

                        refreshTokens.Add(new RefreshToken
                        {
                            Token = seed.Token,
                            ExpireOn = seed.ExpireOn,
                            CreatedOn = seed.CreatedOn,
                            RevokedOn = seed.RevokedOn,
                            RememberMe = seed.RememberMe,
                            ApplicationUserId = user.Id
                        });
                    }

                    context.RefreshTokens.AddRange(refreshTokens);
                    await context.SaveChangesAsync();
                }

                // Add Payments
                if (!context.Payments.Any())
                {
                    var paymentsData = await File.ReadAllTextAsync(Path.Combine(seedFilesPath, "Payments.json"));
                    var payments = JsonSerializer.Deserialize<List<Payment>>(paymentsData, option)!;
                    context.Payments.AddRange(payments);
                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                try
                {
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Users', RESEED, 0)");
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Patients', RESEED, 0)");
                    await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Departments', RESEED, 0)");
                }
                catch (Exception innerEx)
                {
                    Console.WriteLine($"Failed to reset identities: {innerEx.Message}");
                }

                throw;
            }
        }
    }
    public class UserRoleMapping
    {
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }
}