
Select * from [dbo].[Brands]
Select * from [dbo].[PhoneModels]
Select * from[dbo].[Phones]
Select * from[dbo].[StockDetails]
go

Create proc spInsertPhone @PhoneName Nvarchar(50),
						  @ReliseDate DateTime,
						  @IsOfficial bit,
						  @Picture Nvarchar(Max),
						  @PhoneModelId int,
						  @BrandId int

As 
Begin
	Insert into Phones(PhoneName,ReliseDate,IsOfficial,Picture,PhoneModelId,BrandId)
	Values(@PhoneName,@ReliseDate,@IsOfficial,@Picture,@PhoneModelId,@BrandId)
	Select SCOPE_IDENTITY() As PhoneId
	Return
End
Go
--Test Procedure
Exec spInsertPhone 'Samsung S22FE','01-01-2024',1,'Phn2.jpg',1,1
Exec spInsertPhone 'Note 10 Lite','2022-01-10',2,'Ml.jpg',1,1
Go

Create proc spInsertStock 
						  @Color Int,
						  @Price Decimal(18,2),
						  @Quantity int,
						  @PhoneId int
As 
Begin
Insert into StockDetails (Color,Price,Quantity,PhoneId)
	Values (@Color,@Price,@Quantity,@PhoneId)
	Select SCOPE_IDENTITY() As StockDetailsId
End
Go
---Test Procedure
Exec spInsertStock 1,22000,4,1
go

Create Proc spDeleteStock @PhoneId int
As
Begin
	Delete From StockDetails
	Where PhoneId=@PhoneId
	Return
End
Go