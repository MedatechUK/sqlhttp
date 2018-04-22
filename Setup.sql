
SET ANSI_NULLS OFF
SET QUOTED_IDENTIFIER OFF
GO

CREATE ASSEMBLY [MedatechCLR]
FROM 'M:\components\sqlPOST\sqlPOST\bin\Debug\MedatechCLR.dll'
WITH PERMISSION_SET = EXTERNAL_ACCESS

GO

CREATE FUNCTION [dbo].[postxml](@url [nvarchar](max), @xml [xml])
RETURNS [xml] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [MedatechCLR].[sqlhttp.UserDefinedFunctions].[postxml]
GO

CREATE FUNCTION [dbo].[getxml](@url [nvarchar](max))
RETURNS [xml] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [MedatechCLR].[sqlhttp.UserDefinedFunctions].[getxml]
GO

