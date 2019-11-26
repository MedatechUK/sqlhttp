Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml
Imports Microsoft.SqlServer.Server
'Imports Newtonsoft.Json

Partial Public Class UserDefinedFunctions
    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function postxml(url As SqlString, xml As SqlXml) As SqlXml

        Dim requestStream As Stream = Nothing
        Dim uploadResponse As HttpWebResponse = Nothing
        Dim uploadRequest As HttpWebRequest = Nothing

        Try
            uploadRequest = CType(HttpWebRequest.Create(CType(url, String)), HttpWebRequest)
            With uploadRequest
                .Method = "POST"
                .Proxy = Nothing
                .ContentType = "text/xml"
            End With

            Dim myEncoder As New Text.ASCIIEncoding
            Dim ms As MemoryStream = New MemoryStream(myEncoder.GetBytes(xml.Value))

            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            requestStream.Close()
            uploadResponse = CType(uploadRequest.GetResponse(), HttpWebResponse)

            Return New SqlXml(uploadResponse.GetResponseStream)

        Catch ex As Exception
            Throw New Exception(ex.Message)

        End Try

    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function postchar(url As SqlString, poststr As SqlString) As SqlString

        Dim requestStream As Stream = Nothing
        Dim uploadResponse As HttpWebResponse = Nothing
        Dim uploadRequest As HttpWebRequest = Nothing

        Try
            uploadRequest = CType(HttpWebRequest.Create(CType(url, String)), HttpWebRequest)
            With uploadRequest
                .Method = "POST"
                .Proxy = Nothing
                .ContentType = "text/xml"
            End With

            Dim myEncoder As New Text.ASCIIEncoding
            Dim ms As MemoryStream = New MemoryStream(myEncoder.GetBytes(poststr.Value))

            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            requestStream.Close()
            uploadResponse = CType(uploadRequest.GetResponse(), HttpWebResponse)

            Dim sr As New StreamReader(uploadResponse.GetResponseStream)
            With sr
                Return .ReadToEnd
            End With

        Catch ex As Exception
            Throw New Exception(ex.Message)

        End Try

    End Function

    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function getxml(url As SqlString) As SqlXml
        Try
            Dim requestStream As Stream = Nothing
            Dim uploadResponse As HttpWebResponse = Nothing
            Dim uploadRequest As HttpWebRequest = CType(HttpWebRequest.Create(CType(url, String)), HttpWebRequest)
            uploadRequest.Method = "GET"
            uploadRequest.Proxy = Nothing

            uploadResponse = CType(uploadRequest.GetResponse(), HttpWebResponse)

            Return New SqlXml(uploadResponse.GetResponseStream)


        Catch ex As Exception
            Throw New Exception(ex.Message)

        End Try

    End Function

    ' Newtonsoft not supported :(
    '<Microsoft.SqlServer.Server.SqlFunction()>
    'Public Shared Function postjson(url As SqlString, xml As SqlXml) As SqlXml
    '    Dim myEncoder As New Text.ASCIIEncoding
    '    Dim requestStream As Stream = Nothing
    '    Dim uploadResponse As HttpWebResponse = Nothing
    '    Dim uploadRequest As HttpWebRequest = Nothing

    '    Try
    '        uploadRequest = CType(HttpWebRequest.Create(CType(url, String)), HttpWebRequest)
    '        With uploadRequest
    '            .Method = "POST"
    '            .Proxy = Nothing
    '            .ContentType = "text/json"
    '        End With

    '        Dim doc As New XmlDocument
    '        doc.LoadXml(xml.Value)
    '        Dim ms As MemoryStream = New MemoryStream(myEncoder.GetBytes(JsonConvert.SerializeXmlNode(doc)))

    '        requestStream = uploadRequest.GetRequestStream()

    '        ' Upload the XML
    '        Dim buffer(1024) As Byte
    '        Dim bytesRead As Integer
    '        While True
    '            bytesRead = ms.Read(buffer, 0, buffer.Length)
    '            If bytesRead = 0 Then
    '                Exit While
    '            End If
    '            requestStream.Write(buffer, 0, bytesRead)
    '        End While

    '        requestStream.Close()

    '        Dim xmlStream As New MemoryStream
    '        uploadResponse = CType(uploadRequest.GetResponse(), HttpWebResponse)

    '        doc = JsonConvert.DeserializeXmlNode(New StreamReader(uploadResponse.GetResponseStream).ReadToEnd)
    '        doc.Save(xmlStream)

    '        Return New SqlXml(xmlStream)

    '    Catch ex As Exception
    '        Throw New Exception(ex.Message)

    '    Finally
    '        If Not uploadResponse Is Nothing Then uploadResponse.Close()

    '    End Try

    'End Function

End Class
