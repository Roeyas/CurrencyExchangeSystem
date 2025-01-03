CREATE DATABASE CurrencyExchangeDB;

USE [CurrencyExchange]
GO
/****** Object:  Table [dbo].[CurrencyRates]    Script Date: 12/28/2024 4:14:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrencyRates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PairName] [nvarchar](10) NULL,
	[PairValue] [decimal](18, 3) NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetCurrencyRates]    Script Date: 12/28/2024 4:14:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCurrencyRates]
AS

BEGIN
	SELECT 
		PairName,
		PairValue,
		UpdatedDate 
	FROM CurrencyExchange..CurrencyRates
END
GO
/****** Object:  StoredProcedure [dbo].[SaveRates]    Script Date: 12/28/2024 4:14:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRates]
@PairName VARCHAR(30),  
@Value NVARCHAR(30),
@UpdatedDate DATETIME
AS

IF @PairName <> '' AND EXISTS(SELECT 1 FROM CurrencyExchange..CurrencyRates WHERE PairName = @PairName)
BEGIN
	UPDATE CurrencyExchange..CurrencyRates
	SET
		PairValue = @Value,
		UpdatedDate = @UpdatedDate
	WHERE PairName = @PairName
END
ELSE IF @PairName <> ''
BEGIN
	INSERT INTO CurrencyExchange..CurrencyRates(PairName, PairValue, UpdatedDate)
	VALUES (@PairName, @Value, @UpdatedDate)
END
GO
