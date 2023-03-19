using HandmadeShop.Data.Static;
using HandmadeShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandmadeShop.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Cinema
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(new List<Category>()
                    {
                        new Category()
                        {
                            Name= "Decor",
                        }, new Category()
                        {
                            Name= "Đồ chơi"
                        }, new Category()
                        {
                            Name= "Trang phục"
                        }, new Category()
                        {
                            Name= "Trang sức"
                        }, new Category()
                        {
                            Name= "Khác"
                        },

                    });
                    context.SaveChanges();
                }
              
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Handmade 1",
                            Price = 50000,
                            Seller = "Lunami",
                            Description = "Đây là mô tả của Handmade 1",
                            CategoryID = 2,
                            Material = "Len"
                        }, new Product()
                        {
                            Name = "Handmade 2",
                            Price = 100000,
                            Seller = "Lunami",
                            Description = "Đây là mô tả của Handmade 2",
                            CategoryID = 2,
                            Material = "Kim loại, đèn led"
                        }, new Product()
                        {
                            Name = "Handmade 3",
                            Price = 30000,
                            Seller = "Lunami",
                            Description = "Đây là mô tả của Handmade 3",
                            CategoryID = 2,
                            Material = "Kim loại"
                        }, new Product()
                        {
                            Name = "Handmade 4",
                            Price = 60000,
                            Seller = "Lunami",
                            Description = "Đây là mô tả của Handmade 4",
                            CategoryID = 2,
                            Material = "Vải"
                        }, 

                    });
                    context.SaveChanges();
                }
                
                if (!context.ProductImages.Any())
                {
                    context.ProductImages.AddRange(new List<ProductImage>()
                    {
                        new ProductImage()
                        {
                            ProductID = 1,
                            ImagePath = "handmade1-1.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 1,
                            ImagePath = "handmade1-2.jpg"
                        },
                         new ProductImage()
                        {
                            ProductID = 1,
                            ImagePath = "handmade1-3.jpg"
                        },
                          new ProductImage()
                        {
                            ProductID = 2,
                            ImagePath = "handmade2-1.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 2,
                            ImagePath = "handmade2-2.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 2,
                            ImagePath = "handmade2-3.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 3,
                            ImagePath = "handmade3-1.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 3,
                            ImagePath = "handmade3-2.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 3,
                            ImagePath = "handmade3-3.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 4,
                            ImagePath = "handmade4-1.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 4,
                            ImagePath = "handmade4-2.jpg"
                        }, new ProductImage()
                        {
                            ProductID = 4,
                            ImagePath = "handmade4-3.jpg"
                        },

                    });
                    context.SaveChanges();
                }
           
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
               
                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@sqlshop.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Son Nguyen",
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        PhoneNumber = "84817702334"
                    };
                    var result = await userManager.CreateAsync(newAdminUser, "Sonbeo22@123?");

                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }
               

                string appUserEmail = "lunami@sqlshop.com";
               

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Ngoc Quynh",
                        UserName = "lunami",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        PhoneNumber= "84965736750",
                    };
                    var result = await userManager.CreateAsync(newAppUser, "Sonbeo22@123?");

                    if(result.Succeeded) 
                        await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
