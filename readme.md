<h1>sqlhttp CLR</h1>

<h2>Summary</h2>
An SQL CLR providing Native http GET/POST functionality for MSSQL.

<a href="https://github.com/SimonBarnett/sqlhttp/blob/master/Setup.sql"><h2>Installation</h2></a>

<h2>Examples</h2>
<h3>dbo.getxml</h3>
<code>
declare @response xml
select @response = dbo.getxml('{http://endpoint.ashx}')
</code>

<li><a href="https://github.com/SimonBarnett/sqlhttp/blob/master/getTest.sql">GET</a>
<li><a href="https://github.com/SimonBarnett/sqlhttp/blob/master/postTest.sql">POST</a>
