'required for database coding

Imports System.Data.OleDb

Public Class Form3

    'database connection object

    Private CN As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Stock2.mdb;")

    'Moves data between program and database

    Private DA As OleDbDataAdapter

    'Data structure to store records from the DB

    Private DT As New DataTable

    Private Sub Form3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'set up properties of the DataGridView

        InitDGV()

        'get list of all Suppliers

        GetAllSuppliers()

        'get Item records from DB

        ReadDB()

    End Sub

    'Set up properties of DataGridView

    Private Sub InitDGV()

        With DGV

            .Font = New Font(Me.Font.Name, 10, FontStyle.Regular)

            .RowHeadersVisible = False

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader

            .AllowUserToAddRows = False

            .AllowUserToDeleteRows = False

            .ReadOnly = True

        End With

    End Sub

    Private Sub GetAllSuppliers()

        'SQL string to get first two fields

        Dim strSQL As String = "SELECT SupplierID, Name FROM tblSupplier ORDER BY SupplierID"

        'get records from database

        CN.Open()

        Dim CM As New OleDbCommand(strSQL, CN)

        Dim DR As OleDbDataReader = CM.ExecuteReader

        'for each record

        Do While DR.Read

            'add to ComboBox

            cboSuppAll.Items.Add(DR.Item("SupplierID").ToString & " " & DR.Item("Name").ToString)

        Loop

        CN.Close()

        'select top row of ComboBox

        cboSuppAll.SelectedIndex = 0

    End Sub

    'Get records from DB

    Private Sub ReadDB()

        'Select all fields query string

        Dim strSQL As String = "SELECT * FROM tblItem ORDER BY StockID"

        DA = New OleDbDataAdapter(strSQL, CN)

        DA.Fill(DT)

        'display Items in Listbox and select row 0

        FillList(0)

    End Sub

    'display Items in Listbox

    Private Sub FillList(ByVal intSelectRow As Integer)

        'initialise empty

        ListBox1.Items.Clear()

        'use contents of DataTable

        For Each DR As DataRow In DT.Rows

            'add first two fields

            ListBox1.Items.Add(DR.Item("StockID").ToString & " " & DR.Item("Description").ToString)

        Next

        'select row given by parameter

        ListBox1.SelectedIndex = intSelectRow

    End Sub

    'runs automatically when selected row of ListBox changes

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        'get copy of row

        Dim DR As DataRow = DT(ListBox1.SelectedIndex)

        'display fields in TextBoxes

        txtStockID.Text = DR.Item("StockID").ToString

        txtDescription.Text = DR.Item("Description").ToString

        txtPrice.Text = DR.Item("Price").ToString

        txtQuantityInStock.Text = DR.Item("QuantityInStock").ToString

        txtReorderLevel.Text = DR.Item("ReorderLevel").ToString

        txtReorderQuantity.Text = DR.Item("ReorderQuantity").ToString

        'use StockID to refresh list of Suppliers

        GetItemSuppliers(txtStockID.Text)

        'use StockID to refresh list of Orders for this Item

        GetItemOrders(txtStockID.Text)

    End Sub

    Private Sub GetItemSuppliers(ByVal strID As String)

        'SQL string to get first two fields.

        'WHERE clause matches PK with FK:

        Dim strSQL As String = "SELECT tblSupplier.SupplierID, Name " & "FROM tblSupplier, tblSupplierItem " & "WHERE tblSupplier.SupplierID = " & "tblSupplierItem.SupplierID " & "AND StockID = '" & strID & "'"

        'get records from database into DataReader

        CN.Open()

        Dim CM As New OleDbCommand(strSQL, CN)

        Dim DR As OleDbDataReader = CM.ExecuteReader

        'initialise empty

        cboSuppItem.Items.Clear()

        cboSuppItem.Text = ""

        'for each record

        Do While DR.Read

            'add to ComboBox

            cboSuppItem.Items.Add(DR.Item("SupplierID").ToString & " " & DR.Item("Name").ToString)

        Loop

        CN.Close()

        'if ComboBox not empty

        If cboSuppItem.Items.Count > 0 Then

            'select top row

            cboSuppItem.SelectedIndex = 0

        End If

    End Sub

    Private Sub GetItemOrders(ByVal strID As String)

        'Get required fields from Order table.

        'WHERE clause matches PK with FK:

        Dim strSQL As String = "SELECT tblOrder.OrderID, SupplierID, " & "DateSent, DateReceived, Qty " & "FROM tblOrder, tblOrderItem " & "WHERE tblOrder.OrderID = " & "tblOrderItem.OrderID " & "AND StockID = '" & strID & "'"

        'get records from DB into DataTable

        Dim DT2 As New DataTable

        Dim DA2 As New OleDbDataAdapter(strSQL, CN)

        DA2.Fill(DT2)

        'display records in DataGridView

        DGV.DataSource = DT2

        'stop it going blue

        DGV.CurrentCell = Nothing

    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click

        'Create new row for DataTable

        Dim DR As DataRow = DT.NewRow

        'Fill fields from TextBoxes

        DR("StockID") = txtStockID.Text

        DR("Description") = txtDescription.Text

        DR("Price") = CSng(txtPrice.Text)

        DR("QuantityInStock") = CInt("0" & txtQuantityInStock.Text)

        DR("ReorderLevel") = CInt("0" & txtReorderLevel.Text)

        DR("ReorderQuantity") = CInt("0" & txtReorderQuantity.Text)

        'Add row to DataTable

        DT.Rows.Add(DR)

        'update list in ListBox and select bottom row

        FillList(ListBox1.Items.Count - 1)

    End Sub

    Private Sub btnAmend_Click(sender As System.Object, e As System.EventArgs) Handles btnAmend.Click

        'Memorise which record selected

        Dim intR As Integer = ListBox1.SelectedIndex

        'update fields from TextBoxes

        DT(intR)("StockID") = txtStockID.Text

        DT(intR)("Description") = txtDescription.Text

        DT(intR)("Price") = CSng(txtPrice.Text)

        DT(intR)("QuantityInStock") = CInt("0" & txtQuantityInStock.Text)

        DT(intR)("ReorderLevel") = CInt("0" & txtReorderLevel.Text)

        DT(intR)("ReorderQuantity") = CInt("0" & txtReorderQuantity.Text)

        'update list in ListBox and select same row

        FillList(intR)

    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

        Dim CB As OleDbCommandBuilder = New OleDbCommandBuilder(DA)

        DA.Update(DT)

    End Sub

    Private Sub btnInsert_Click(sender As System.Object, e As System.EventArgs) Handles btnInsert.Click

        'Get data from boxes and quit if empty:

        Dim strStockID As String = txtStockID.Text

        If strStockID = "" Then Exit Sub

        Dim strSupplierID As String = cboSuppItem.Text.Substring(0, cboSuppItem.Text.IndexOf(Chr(32)))

        If strSupplierID = "" Then Exit Sub

        'get cost from TextBox with default value of 0

        'might be better to use an InputBox here

        Dim sngCost As Single = CSng("0" & txtPrice.Text)

        'Add new record to table

        Dim strSQL As String = "INSERT INTO tblSupplierItem VALUES('" & strSupplierID & "','" & strStockID & "'," & sngCost & ")"

        CN.Open()

        Dim CM As New OleDbCommand(strSQL, CN)

        Try

            CM.ExecuteNonQuery()

            MessageBox.Show("Succeeded")

        Catch ex As Exception

            MessageBox.Show("Failed:" & vbCrLf & ex.Message)

        Finally

            CN.Close()

        End Try

        'Refresh ComboBox

        GetItemSuppliers(strStockID)

    End Sub

    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

        'Get data from boxes and quit if empty:

        Dim strStockID As String = txtStockID.Text

        If strStockID = "" Then Exit Sub

        Dim strSupplierID As String = cboSuppItem.Text.Substring(0, 2)

        If strSupplierID = "" Then Exit Sub

        'Remove record from table

        Dim strSQL As String = "DELETE FROM tblSupplierItem " & "WHERE StockID = '" & strStockID & "' " & "AND SupplierID = '" & strSupplierID & "'"

        CN.Open()

        Dim CM As New OleDbCommand(strSQL, CN)

        Try

            CM.ExecuteNonQuery()

            MessageBox.Show("Succeeded")

        Catch ex As Exception

            MessageBox.Show("Failed:" & vbCrLf & ex.Message)

        Finally

            CN.Close()

        End Try

        'Refresh ComboBox

        GetItemSuppliers(strStockID)

    End Sub
End Class