Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Net
Imports Microsoft.SqlServer.Server

Partial Public Class UserDefinedFunctions
    <Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function postxml(url As SqlString, xml As SqlXml) As SqlXml
        Try
            Dim requestStream As Stream = Nothing
            Dim uploadResponse As HttpWebResponse = Nothing
            Dim uploadRequest As HttpWebRequest = CType(HttpWebRequest.Create(CType(url, String)), HttpWebRequest)
            uploadRequest.Method = "POST"
            uploadRequest.Proxy = Nothing

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

End Class
