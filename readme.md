<h1>sqlHttp CLR library</h1>

<h2>Summary</h2>
An SQL CLR providing Native http GET/POST functionality for MSSQL.

<h2>Examples</h2>
<h3>dbo.getxml</h3>
GET the response from an XML REST Endpoint as XMLSQL type.

```sql
	declare @response xml
	select @response = dbo.getxml('{http://endpoint.ashx}')
```
<h3>dbo.postchar</h3>
POST VARCHAR type data to a REST Endpoint.

```sql
	declare @response VARCHAR(MAX)
	select @response = dbo.postchar('{http://endpoint.ashx}', '{String_Data}')	
```

<h3>dbo.postxml</h3>
POST XMLSQL type data to an XML REST Endpoint.
The XML data may come from a SQL type feed, or from the result of a get/post.

```sql
	declare @response xml
	select @response = dbo.postxml('{http://endpoint.ashx}', {SQLXML_Data})	
```

<h3>Handle responses in MS-SQL</h3>
It is possible to SQL SELECT from the results.

```sql
	declare @response xml
	declare @hdoc int

	BEGIN TRY
		select @response = dbo.postxml({http://endpoint.ashx}, dbo.xmlCustomers('GenRep'))	
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @response
	
		select	
			[status],
			[message]
		FROM OPENXML(@hdoc, 'response',8)
		with (
			[status] int,
			[message] varchar(max),
			[stacktr] varchar(max)
		)
	
		EXEC sp_xml_removedocument @hdoc
	
	END TRY

	BEGIN CATCH
		SELECT 500, ERROR_MESSAGE()
	
	END CATCH
```

<h3>Load responses with a handler endpoint</h3>
Alternatively you can send the response to a handler endpoint to load the data into Priority.

```sql
	declare @response xml
	declare @hdoc int

	BEGIN TRY
		-- Send GET result to Handler
		select @response = dbo.postxml(
			'{http://destination_endpoint.ashx}', 
			(dbo.getxml('{http://source_endpoint.ashx}))
		)
		EXEC sp_xml_preparedocument @hdoc OUTPUT, @response
	
		select	
			[status],
			[message]
		FROM OPENXML(@hdoc, 'response',8)
		with (
			[status] int,
			[message] varchar(max),
			[stacktr] varchar(max)
		)

		EXEC sp_xml_removedocument @hdoc
	
	END TRY

	BEGIN CATCH
		SELECT 500, ERROR_MESSAGE()

	END CATCH
```

<h2>Set-up the sqlHttp CLR library</h2>

```sql
	SET ANSI_NULLS OFF
	SET QUOTED_IDENTIFIER OFF
	GO

	CREATE ASSEMBLY [MedatechCLR]
	FROM '{path_to_dll}\MedatechCLR.dll'
	WITH PERMISSION_SET = EXTERNAL_ACCESS

	GO

	CREATE FUNCTION [dbo].[postchar](@url [nvarchar](max), @poststr [nvarchar](max))
	RETURNS [nvarchar](max) WITH EXECUTE AS CALLER
	AS 
	EXTERNAL NAME [MedatechCLR].[sqlhttp.UserDefinedFunctions].[postchar]
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
```