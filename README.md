ask: Build a Web API for an E-
Commerce System 
 
1. Models and Relationships 
Design the following models: 
 
1.  User 
o Properties: 
▪ Id (int, primary key) 
▪ Name (string, required) 
▪ Email (string, unique, regex) 
▪ Password (string, regex) 
▪ Phone (string, required) 
▪ Role (String, required) 
▪ CreatedAt (DateTime) 
o Relationships: 
▪ A user can place many 
orders. 
▪ A user can write many 
reviews. 
2.  Product 
o Properties: 
▪ Id (int, primary key) 
▪ Name (string, required) 
▪ Description (string) 
▪ Price (decimal, required, > 0) 
▪ Stock (int, required, >= 0) 
▪ Overall Rating (decimal) 
Calculated reviews ratings 
o Relationships: 
▪ A product can have many 
reviews. 
3.  Order 
o Properties: 
▪ Id (int, primary key) 
▪ UserId (foreign key to User) 
▪ OrderDate (DateTime) 
▪ TotalAmount (decimal, 
calculated based on products 
in the order) 
o Relationships: 
▪ An order can contain 
multiple products. 
▪ Many-to-many relationship 
with Product via 
OrderProduct. 
 
4.  OrderProducts (junction table) 
o Properties: 
▪ OrderId (foreign key to 
Order) 
▪ ProductId (foreign key to 
Product) 
▪ Quantity (int, required, > 0) 
5.  Review 
o Properties: 
▪ Id (int, primary key) 
▪ UserId (foreign key to User) 
▪ ProductId (foreign key to 
Product) 
▪ Rating (int, required, 1 to 5) 
▪ Comment (string, optional) 
▪ ReviewDate (DateTime) 
o Relationships: 
▪ A review is linked to one 
product and one user. 
2. APIs to Implement 
1.  User APIs (Authorized) 
o Register a new user. (Allow) 
o Login (validate email and 
(Allow)(password, return a token) 
o Get user details by ID 
(authenticated users only)  
2.  Product APIs (Authorized ) 
o Add a new product Add a new 
product (Admin only).  
o Update product details (Admin 
only) 
o Get a list of products with 
pagination and filtering by 
name/price range.  
o Get product details by ID. 
3.  Order APIs (Authorized) 
o Place a new order: 
▪ Calculate the total amount 
based on product prices and 
quantities. 
▪ Reduce the stock of ordered 
products. 
o Get all orders for a user 
(authenticated users only) 
o Get order details by ID 
(authenticated users only).  
4.  Review APIs (Authorized) 
o Add a review for a product 
(authenticated users only, product 
must be part of a previous order for 
this user). 
o Get all reviews for a product with 
pagination. 
o Update or delete a review (only by 
the user who created it). 
3. Business Rules and Conditions 
1.  Users: 
o A user’s email must be unique and 
has email format. 
o Passwords should be hashed when 
stored. 
2.  Products: 
o Product price must be greater than 
zero. 
o Stock cannot be negative. 
3.  Orders: 
o Orders can only be placed if 
sufficient stock is available for all 
items. 
o The total amount is automatically 
calculated. 
o Once an order is placed, the 
product stock is reduced 
accordingly. 
4.  Reviews: 
o A user can only review a product 
they have purchased. 
o A user cannot review the same 
product more than once. 
o Rating must be between 1 and 5. 
o Once review placed, recalculate 
product overall rating 
 
