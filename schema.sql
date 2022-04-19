CREATE DATABASE checkout

CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL,
	[CardNumber] [nvarchar](max) NOT NULL,
	[ExpiryMonth] [int] NOT NULL,
	[ExpiryYear] [int] NOT NULL,
	[CVV] [nvarchar](max) NOT NULL,
	[Currency] [nvarchar](max) NOT NULL,
	[Amount] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payments] ADD  CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

CREATE TABLE [dbo].[Authorizations](
	[Id] [uniqueidentifier] NOT NULL,
	[PaymentId] [uniqueidentifier] NOT NULL,
	[Approved] [bit] NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[ResponseCode] [nvarchar](max) NOT NULL,
	[ResponseSummary] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Scheme] [nvarchar](max) NOT NULL,
	[Last4] [nchar](4) NOT NULL,
	[Bin] [nvarchar](max) NOT NULL,
	[CardType] [nvarchar](max) NOT NULL,
	[Issuer] [nvarchar](max) NOT NULL,
	[IssuerCountry] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Authorizations] ADD  CONSTRAINT [PK_Authorizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Authorizations] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO

CREATE TABLE [dbo].[IdempotencyTokens](
	[MessageId] [uniqueidentifier] NOT NULL,
	[Consumer] [nvarchar](450) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[IdempotencyTokens] ADD CONSTRAINT [PK_IdempotencyTokens] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC,
	[Consumer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IdempotencyTokens] ADD  DEFAULT (getutcdate()) FOR [CreatedOn]
GO