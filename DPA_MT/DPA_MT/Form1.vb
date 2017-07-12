'required for database coding

Imports System.Data.OleDb

Public Class Form1

    Private Sub btnCreateDB_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateDB.Click

        'quit if user has not supplied a database name:

        If txtDBName.Text = "" Then Exit Sub

        Dim strDBName As String = txtDBName.Text

        'catalog object needed to create new database:

        Dim CAT As New ADOX.Catalog()

        Try

            CAT.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strDBName)

            MessageBox.Show("Succeeded")

        Catch ex As Exception

            MessageBox.Show("Failed:" & vbCrLf & ex.Message)

        Finally

            CAT = Nothing

        End Try

    End Sub

    Private Sub btnCreateTable_Click(sender As System.Object, e As System.EventArgs) Handles btnCreateTable.Click

        'quit if user has not supplied data:

        If txtDBName.Text = "" Then Exit Sub

        If txtTableName.Text = "" Then Exit Sub

        If txtDDL.Text = "" Then Exit Sub

        'get strings from boxes

        Dim strDBName As String = txtDBName.Text

        Dim strTableName As String = txtDBName.Text

        Dim strDDL As String = txtDDL.Text

        'create and open connection object:

        Dim CON As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strDBName)

        CON.Open()

        'run command using script:

        RunCMD(strDDL, CON)

        'tidy up:

        CON.Close()

        CON.Dispose()

    End Sub

    Private Sub RunCMD(ByVal strDDL As String, ByRef CON As OleDbConnection)

        'create command object

        Dim CMD As New OleDbCommand(strDDL, CON)

        Try

            'run command if syntax correct:

            CMD.ExecuteNonQuery()

            MessageBox.Show("Succeeded")

        Catch ex As Exception

            'otherwise show error message:

            MessageBox.Show("Failed:" & vbCrLf & ex.Message)

        Finally

            CMD.Dispose()

        End Try

    End Sub

    Private Sub btnPopulate_Click(sender As System.Object, e As System.EventArgs) Handles btnPopulate.Click

        'quit if user has not supplied data:

        If txtDBName.Text = "" Then Exit Sub

        If txtTableName.Text = "" Then Exit Sub

        If txtData.Text = "" Then Exit Sub

        'get strings from boxes

        Dim strDBName As String = txtDBName.Text

        Dim strTableName As String = txtTableName.Text

        Dim strData As String = txtData.Text

        'needed to transfer data from TextBox to DataTable:

        Dim strRecs(), strFields() As String

        Dim intNumRecs, intNumFields, i, j As Integer

        'create connection:

        Dim CON As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strDBName)

        'create copy of table:

        Dim DA As New OleDbDataAdapter("SELECT * FROM " & strTableName, CON)

        Dim DT As New DataTable

        DA.Fill(DT)

        'put each row of text into a separate record.

        'have to replace CR/LF pair (string) with single character

        strRecs = strData.Replace(vbCrLf, Chr(8)).Split(Chr(8))

        intNumRecs = strRecs.Length

        'for each record

        For i = 0 To intNumRecs - 1

            'split record into fields using vertical bars:

            strFields = strRecs(i).Split(Chr(124))

            'should be the same every time:

            intNumFields = strFields.Length

            'add new row to DataTable:

            DT.Rows.Add()

            'for each field

            For j = 0 To intNumFields - 1

                'copy fields into DataTable:

                DT(i)(j) = strFields(j)

            Next

        Next

        'send table back to database:

        Dim CB As New OleDbCommandBuilder(DA)

        DA.Update(DT)

        'tidy up:

        DT.Dispose()

        DA.Dispose()

        CON.Dispose()

    End Sub
End Class