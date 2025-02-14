CREATE DATABASE ECommerece

 CREATE TABLE [User] (
    UserId INT PRIMARY KEY,
	MainAddressId INT,
    FName NVARCHAR(50),
    LName NVARCHAR(50),
    [Password] VARCHAR(50),
    Email NVARCHAR(50),
    Phone NVARCHAR(50),
    [Role] NVARCHAR(50)
);

CREATE TABLE Address (
    AddressId INT PRIMARY KEY,
    UserId INT,
    City NVARCHAR(50),
    ZipCode NVARCHAR(50),
    [State] VARCHAR(50),
    Street NVARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
);

ALTER TABLE [User]
ADD CONSTRAINT FK_User_Address
FOREIGN KEY (MainAddressId) REFERENCES Address(AddressId)
ON DELETE SET NULL;

CREATE TABLE Token (
    TokenId INT PRIMARY KEY,
    UserId INT,
    DeviceDetails NVARCHAR(50),
    SecretKey NVARCHAR(50),
    ExpiryDate DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
);

CREATE TABLE Category (
    CategoryId INT PRIMARY KEY,
    Name NVARCHAR(50),
    Description NVARCHAR(50)
    ImagePath NVARCHAR(50)
);

CREATE TABLE Product (
    ProductId INT PRIMARY KEY,
    CategoryId INT,
    Name NVARCHAR(50),
    Description NVARCHAR(50),
    Quantity INT,
    Status NVARCHAR(50),
    Price DECIMAL(18, 2),
    ViewsCount INT,
    PurchaseCount INT,
    ReviewsCount INT,
    AverageReviews DECIMAL(18, 2),
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
)

CREATE Table FavouriteList (
    UserId INT,
    ProductId INT,
    PRIMARY KEY (UserId, ProductId),
    FOREIGN KEY (UserId) REFERENCES [User](UserId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);

CREATE TABLE Reviews(
    ReviewId INT PRIMARY KEY,
    UserId INT,
    ProductId INT,
    Rating INT,
    Review NVARCHAR(50),
    Date DATETIME,
    VerifiedPurchase BIT,
    FOREIGN KEY (UserId) REFERENCES [User](UserId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
)
