Imports System.Xml

Module XMLstuff
    Private Sub XmlKeeping()
        Dim rawData As String = _
                "<Products>" & _
                "  <Product>" & _
                "    <name>Name 1</name>" & _
                "    <Id>101</Id>" & _
                "    <quantity>10</quantity>" & _
                "  </Product>" & _
                "  <Product>" & _
                "    <name>Name 2</name>" & _
                "    <Id>102</Id>" & _
                "    <quantity>10</quantity>" & _
                "  </Product>" & _
                "</Products>"

        Dim xmlDoc As New XmlDocument
        Dim productNodes As XmlNodeList
        Dim productNode As XmlNode
        Dim baseDataNodes As XmlNodeList
        Dim bFirstInRow As Boolean

        xmlDoc.LoadXml(rawData)

        productNodes = xmlDoc.GetElementsByTagName("Product")
        For Each productNode In productNodes
            baseDataNodes = productNode.ChildNodes
            bFirstInRow = True
            For Each baseDataNode As XmlNode In baseDataNodes
                If (bFirstInRow) Then
                    bFirstInRow = False
                Else
                    Console.Write(", ")
                End If
                Console.Write(baseDataNode.Name & ": " & baseDataNode.InnerText)
            Next
        Next
    End Sub
End Module
