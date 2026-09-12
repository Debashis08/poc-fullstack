CREATE TABLE [dbo].[tbl_products] (
    ProductId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(500) NULL,
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ImageUrl NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL 
        CONSTRAINT DF_tbl_products_CreatedAt 
        DEFAULT SYSUTCDATETIME()
);