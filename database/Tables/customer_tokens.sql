CREATE TABLE [dbo].[tbl_customer_refresh_tokens]
(
    RefreshTokenId BIGINT IDENTITY(1,1) NOT NULL,
    CustomerId BIGINT NOT NULL,
    TokenHash NVARCHAR(500) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_tbl_customer_refresh_tokens_CreatedAt
        DEFAULT SYSUTCDATETIME(),
    RevokedAt DATETIME2 NULL,

    CONSTRAINT PK_tbl_customer_refresh_tokens
        PRIMARY KEY (RefreshTokenId),
        
    CONSTRAINT FK_tbl_customer_refresh_tokens_Customers
        FOREIGN KEY (CustomerId)
        REFERENCES [dbo].[tbl_customers](CustomerId),
        
    CONSTRAINT UQ_tbl_customer_refresh_tokens_CustomerId
        UNIQUE (CustomerId)
);