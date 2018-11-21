<h1>sqlHttp CLR library</h1>

<h2>Summary</h2>
An SQL CLR providing Native http GET/POST functionality for MSSQL.

<h2>Examples</h2>
<h3>dbo.getxml</h3>
GET the response from an XML REST Endpoint as XMLSQL type.
<code>
declare @response xml<br>
select @response = dbo.getxml('{http://endpoint.ashx}')<br>
</code>

<h3>dbo.postxml</h3>
POST XMLSQL type data to an XML REST Endpoint.<br>
The XML data may come from a SQL type feed.
<code>
declare @response xml<br>
select @response = dbo.postxml('{http://endpoint.ashx}', {SQLXML_Data})	<br>
</code>

<h3>Handle responses in MS-SQL</h3>
It is possible to SQL SELECT from the results.
<code>
declare @response xml<br>
declare @hdoc int<br>
<br>
BEGIN TRY<br>
	select @response = dbo.postxml({http://endpoint.ashx}, dbo.xmlCustomers('GenRep'))	<br>
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @response<br>
	<br>
	select	<br>
		[status],<br>
		[message]<br>
	FROM OPENXML(@hdoc, 'response',8)<br>
	with (<br>
		[status] int,<br>
		[message] varchar(max),<br>
		[stacktr] varchar(max)<br>
	)<br>
	<br>
	EXEC sp_xml_removedocument @hdoc<br>
	<br>
END TRY<br>
<br>
BEGIN CATCH<br>
	SELECT 500, ERROR_MESSAGE()<br>
	<br>
END CATCH<br>
</code>

<h3>Load responses with a handler endpoint</h3>
Alternatively you can send the response to a handler endpoint to load the data into Priority.
<code>
declare @response xml<br>
declare @hdoc int<br>
<br>
BEGIN TRY<br>
	-- Send GET result to Handler<br>
	select @response = dbo.postxml(<br>
		'{http://destination_endpoint.ashx}', <br>
		(dbo.getxml('{http://source_endpoint.ashx}))<br>
	)<br>
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @response<br>
	<br>
	select	<br>
		[status],<br>
		[message]<br>
	FROM OPENXML(@hdoc, 'response',8)<br>
	with (<br>
		[status] int,<br>
		[message] varchar(max),<br>
		[stacktr] varchar(max)<br>
	)<br>
<br>
	EXEC sp_xml_removedocument @hdoc<br>
	<br>
END TRY<br>
<br>
BEGIN CATCH<br>
	SELECT 500, ERROR_MESSAGE()<br>
<br>
END CATCH<br>
</code>

<h2>Set-up the sqlHttp CLR library</h2>
<code>
SET ANSI_NULLS OFF<br>
SET QUOTED_IDENTIFIER OFF<br>
GO<br>
<br>
CREATE ASSEMBLY [MedatechCLR]<br>
FROM '{path_to_dll}\MedatechCLR.dll'<br>
WITH PERMISSION_SET = EXTERNAL_ACCESS<br>
<br>
GO<br>
<br>
CREATE FUNCTION [dbo].[postxml](@url [nvarchar](max), @xml [xml])<br>
RETURNS [xml] WITH EXECUTE AS CALLER<br>
AS <br>
EXTERNAL NAME [MedatechCLR].[sqlhttp.UserDefinedFunctions].[postxml]<br>
GO<br>
<br>
CREATE FUNCTION [dbo].[getxml](@url [nvarchar](max))<br>
RETURNS [xml] WITH EXECUTE AS CALLER<br>
AS <br>
EXTERNAL NAME [MedatechCLR].[sqlhttp.UserDefinedFunctions].[getxml]<br>
GO<br>
</code>
