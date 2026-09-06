CREATE TABLE [dbo].[tbl_customers]
(
    CustomerId      BIGINT IDENTITY(1,1) PRIMARY KEY,

    Email           NVARCHAR(320) NOT NULL,
    PasswordHash    NVARCHAR(500) NOT NULL,

    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NULL,
    PhoneNumber     NVARCHAR(30) NULL,

    IsActive        BIT NOT NULL DEFAULT 1,

    CreatedAt       DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt       DATETIME2 NULL,

    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);