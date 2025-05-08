CREATE DATABASE Ecommerce;
USE Ecommerce;

CREATE TABLE Users (
    userId INT PRIMARY KEY IDENTITY(1,1),
    firstName NVARCHAR(50) NOT NULL,
    lastName NVARCHAR(50) NOT NULL,
    password VARCHAR(50) NOT NULL,
    email NVARCHAR(50) NOT NULL UNIQUE,
    [role] NVARCHAR(50) NOT NULL DEFAULT 'User',
    address NVARCHAR(255) DEFAULT 'NO Address Available'
);

CREATE TABLE Categories (
    categoryId INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(50) NOT NULL UNIQUE,
    description TEXT DEFAULT 'NO Description Available',
    imagePath NVARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    productId INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(50) NOT NULL UNIQUE,
    description TEXT DEFAULT 'NO Description Available',
    quantity INT DEFAULT 0 CHECK (quantity >= 0),
    status NVARCHAR(20) NOT NULL DEFAULT 'Out Of Stock' CHECK (status IN ('Available', 'Out Of Stock', 'Discontinued')),
    price DECIMAL(10,2) NOT NULL CHECK (price > 0),
    countOfViews INT DEFAULT 0 CHECK (countOfViews >= 0),
    countOfPurchase INT DEFAULT 0 CHECK (countOfPurchase >= 0),
    countOfReview INT DEFAULT 0 CHECK (countOfReview >= 0),
    averageOfReviews DECIMAL(6,2) DEFAULT 0,
    categoryId INT,
    FOREIGN KEY (categoryId) REFERENCES Categories(categoryId) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE Payments (
    transactionId INT PRIMARY KEY IDENTITY(1,1),
    orderId INT NOT NULL,
    amount DECIMAL(10,2) NOT NULL CHECK (amount > 0),
    method NVARCHAR(50) NOT NULL CHECK (method IN ('Credit Card', 'PayPal', 'Cash on Delivery')),
    status NVARCHAR(20) NOT NULL CHECK (status IN ('Completed', 'Pending', 'Failed')),
    date DATETIME DEFAULT GETDATE()
);

CREATE TABLE Reviews (
    reviewId INT PRIMARY KEY IDENTITY(1,1),
    rating INT NOT NULL CHECK (rating BETWEEN 1 AND 5),
    comment NVARCHAR(MAX) DEFAULT 'NO COMMENT',
    reviewDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    userId INT NOT NULL,
    productId INT NOT NULL,
    FOREIGN KEY (userId) REFERENCES Users(userId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productId) REFERENCES Products(productId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE FavouriteLists (
    userId INT NOT NULL,
    productId INT NOT NULL,
    createdAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (userId, productId),
    FOREIGN KEY (userId) REFERENCES Users(userId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productId) REFERENCES Products(productId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Orders (
    orderId INT PRIMARY KEY IDENTITY(1,1),
    userId INT NOT NULL,
    note NVARCHAR(MAX),
    totalPrice DECIMAL(10,2) NOT NULL CHECK (totalPrice > 0),
    date DATETIME DEFAULT GETDATE(),
    status NVARCHAR(50) NOT NULL CHECK (status IN ('Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled', 'Returned')),
    FOREIGN KEY (userId) REFERENCES Users(userId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE OrderItems (
    orderId INT NOT NULL,
    productId INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    totalPrice DECIMAL(10,2) NOT NULL CHECK (totalPrice > 0),
    PRIMARY KEY (orderId, productId),
    FOREIGN KEY (orderId) REFERENCES Orders(orderId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (productId) REFERENCES Products(productId) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Discounts (
    discountId INT PRIMARY KEY IDENTITY(1,1),
    discountStartDate DATE NOT NULL,
    discountEndDate DATE NOT NULL CHECK (discountEndDate >= discountStartDate),
    discountName NVARCHAR(255) NOT NULL,
    productId INT NULL,
    orderId INT NULL,
    FOREIGN KEY (productId) REFERENCES Products(productId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (orderId) REFERENCES Orders(orderId) ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO Users (firstName, lastName, password, email, [role]) VALUES
('John', 'Doe', 'P@ssw0rd123', 'john.doe@example.com', 'User'),
('Jane', 'Smith', 'SecureP@ss456', 'jane.smith@example.com', 'User'),
('Admin', 'User', 'Admin@123', 'admin@techxpress.com', 'Admin'),
('Sarah', 'Johnson', 'Sarah@789', 'sarah.j@example.com', 'User');

SELECT * FROM Users;

INSERT INTO Categories (name, description, imagePath) VALUES
('Laptops', 'High-performance laptops for work and gaming', '/images/categories/laptops.jpg'),
('Smartphones', 'Latest smartphones with advanced features', '/images/categories/smartphones.jpg'),
('Audio', 'High-quality audio devices for immersive experience', '/images/categories/audio.jpg'),
('Accessories', 'Essential accessories for your devices', '/images/categories/accessories.jpg');

SELECT * FROM Categories;

('MacBook Pro 16"', 'Apple MacBook Pro with M2 chip, 16GB RAM, 512GB SSD', 25, 'Available', 2499.99, 1),
('Samsung Galaxy S25', 'Latest Samsung flagship with 8K camera and 5G', 50, 'Available', 1199.99, 2),
('Sony WH-1000XM5', 'Wireless noise-cancelling headphones with 30-hour battery', 35, 'Available', 349.99, 3),
('Dell XPS 15', 'Powerful Windows laptop with 4K display and Intel i9', 15, 'Available', 1999.99, 1);

SELECT * FROM Products;

INSERT INTO Orders (userId, note, totalPrice, status) VALUES
(1, 'Please deliver to reception', 2499.99, 'Processing'),
(2, 'Call before delivery', 1549.98, 'Pending'),
(3, NULL, 349.99, 'Shipped'),
(4, 'Leave at door', 1999.99, 'Delivered');

SELECT * FROM Orders;

INSERT INTO OrderItems (orderId, productId, quantity, totalPrice) VALUES
(1, 1, 1, 2499.99),
(2, 2, 1, 1199.99),
(2, 3, 1, 349.99),
(3, 3, 1, 349.99),
(4, 4, 1, 1999.99);

SELECT * FROM OrderItems;

INSERT INTO Payments (orderId, amount, method, status) VALUES
(1, 2499.99, 'Credit Card', 'Completed'),
(2, 1549.98, 'PayPal', 'Pending'),
(3, 349.99, 'Credit Card', 'Completed'),
(4, 1999.99, 'Cash on Delivery', 'Pending');

SELECT * FROM Payments;

INSERT INTO Reviews (rating, comment, userId, productId) VALUES
(5, 'Excellent laptop, very fast and great display!', 1, 1),
(4, 'Good phone, camera quality could be better.', 2, 2),
(5, 'Best headphones I have ever owned!', 3, 3),
(4, 'Great laptop for work, but runs hot sometimes.', 4, 4);

SELECT * FROM Reviews;

-- Insert sample data into FavouriteLists table
INSERT INTO FavouriteLists (userId, productId) VALUES
(1, 3),
(2, 1),
(3, 4),
(4, 2);

SELECT * FROM FavouriteLists;

INSERT INTO Discounts (discountStartDate, discountEndDate, discountName, productId) VALUES
('2025-05-01', '2025-06-01', 'Spring Sale', 1),
('2025-05-01', '2025-06-01', 'Spring Sale', 2),
('2025-05-15', '2025-06-15', 'Audio Fest', 3),
('2025-05-20', '2025-07-01', 'Summer Deals', 4);

SELECT * FROM Discounts;

SELECT o.orderId, o.date, o.status, u.firstName, u.lastName, u.email, o.totalPrice
FROM Orders o
JOIN Users u ON o.userId = u.userId
ORDER BY o.date DESC;

SELECT p.productId, p.name, p.price, p.status, c.name as categoryName
FROM Products p
JOIN Categories c ON p.categoryId = c.categoryId;

SELECT oi.orderId, oi.quantity, oi.totalPrice, p.name, p.price
FROM OrderItems oi
JOIN Products p ON oi.productId = p.productId
ORDER BY oi.orderId;
