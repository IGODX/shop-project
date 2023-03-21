### Shop 

### Project description:
A simple C# ASP.NET Core project that represent my knowledge of technology. 

### Project contains: CRUD operations,  

### Used technologies:
- C# 6.0
- ASP.NET Core MVC
- Entity Framework 
- OAuth 2.0 ( Google, Facebook )
- JavaScript
- AJAX
- HTML
- CSS

![Project](https://user-images.githubusercontent.com/109869521/226229459-c1b7c525-a71a-44c2-a28c-c1ea9bee52d4.jpg)

### Getting started:
First of all you need to initialize user secrets . You can do it from console or using Visual Studio. <strong>You should :</strong>  
- Initialize 
"ConnectionStrings": {
        "shopDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=shopDb;Integrated Security=True;"
    },
"AdminEmail": "admin@gmail.com",
"AdminPassword": "password",

- Optional values :
"Authentication:Google:ClientSecret": "your value"
    "Authentication:Google:ClientId": "your value"
    "Authentication:Facebook:AppId": "your value"
    "Authentication:Facebook:AppSecret": "your value"
           <strong>It will enable authentication by Google or Facebook.</strong> 
          
### After that, you can run the application. 
To create categories and products you neeed login as admin ( it has been initialized earlier ) and press Admin Controller.

Also, you can edit your products. You can add and delete photos without reloading page ( it implements <strong>AJAX</strong> ).  
### Additional admin's functionality:
- Adding users
- Remove existing users,
- Give another user roles.

### Default user can only see products and add it to the cart.


