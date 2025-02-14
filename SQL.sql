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
ADD CONSTRAINT FK_User_Addresss
FOREIGN KEY (MainAddressId) REFERENCES Address(AddressId);

CREATE TABLE Token (
    TokenId INT PRIMARY KEY,
    UserId INT,
    DeviceDetails NVARCHAR(50),
    SecretKey NVARCHAR(50),
    ExpiryDate DATETIME,
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
);
