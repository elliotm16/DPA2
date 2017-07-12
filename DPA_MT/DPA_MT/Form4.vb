'required for database coding
Imports System.Data.OleDb

Imports System.IO

Public Class Form4

    'database connection object

    Private CN As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Stock2.mdb;")

    'moves data between program and database

    Private DAItems As OleDbDataAdapter

    'data structures to store records from the DB

    Private DTItems, DTNewOrder As DataTable

    Private Sub Form4_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        'set up properties of DataGridViews

        InitDGV1()

        InitDGV2()

        'get list of all Suppliers

        GetAllSuppliers()

        'get Item records from DB

        GetLowStock()

        'show current date

        txtDate.Text = Now.Date.ToShortDateString

    End Sub

    Private Sub InitDGV1()

        With DGV1

            .Font = New Font(Me.Font.Name, 10, FontStyle.Regular)

            .RowHeadersVisible = False

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader

            .AllowUserToAddRows = False

            .AllowUserToDeleteRows = False

            .ReadOnly = True

        End With

    End Sub

    Private Sub InitDGV2()

        With DGV2

            .Font = New Font(Me.Font.Name, 10, FontStyle.Regular)

            .RowHeadersVisible = False

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader

            .AllowUserToAddRows = False

            .AllowUserToDeleteRows = False

            .ReadOnly = False

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

    Private Sub GetLowStock()

        'Select all fields of low stock Items query string

        Dim strSQL As String = "SELECT * FROM tblItem " & "WHERE QuantityInStock < ReorderLevel"

        DAItems = New OleDbDataAdapter(strSQL, CN)

        DTItems = New DataTable

        DAItems.Fill(DTItems)

        'display Items in Listbox and select row 0

        FillList(0)

    End Sub

    'display Items in Listbox

    Private Sub FillList(ByVal intSelectRow As Integer)

        'initialise empty

        ListBox1.Items.Clear()

        ''use contents of DataTable

        'For Each DR As DataRow In DT.Rows

        ''add first two fields

        'ListBox1.Items.Add(DR.Item("StockID").ToString & " " & DR.Item("Description").ToString)

        'Next

        ''select row given by parameter

        ListBox1.SelectedIndex = intSelectRow

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

        'get copy of row

        Dim DR As DataRow = DTItems(ListBox1.SelectedIndex)

        'display fields in TextBoxes

        txtQuantityInStock.Text = DR.Item("QuantityInStock").ToString

        txtReorderLevel.Text = DR.Item("ReorderLevel").ToString

        txtReorderQuantity.Text = DR.Item("ReorderQuantity").ToString

        'use StockID to refresh list of Suppliers

        GetItemSuppliers(DR("StockID").ToString)

        'use StockID to refresh unfilled Orders

        GetUnfilledOrders(DR("StockID").ToString)

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

    Private Sub GetUnfilledOrders(ByVal strID As String)

        'Get required fields from Order table.

        'WHERE clause matches PK with FK:

        Dim strSQL As String = "SELECT tblOrder.OrderID, SupplierID, " & "DateSent, DateReceived, Qty " & "FROM tblOrder, tblOrderItem " & "WHERE tblOrder.OrderID = " & "tblOrderItem.OrderID " & "AND StockID = '" & strID & "' " & "AND DateReceived IS NULL"

        'get records from DB into DataTable

        Dim DTUnfilled As New DataTable

        Dim DAUnfilled As New OleDbDataAdapter(strSQL, CN)

        DAUnfilled.Fill(DTUnfilled)

        'display records in DataGridView

        DGV1.DataSource = DTUnfilled

        'stop it going blue

        DGV1.CurrentCell = Nothing

    End Sub

    Private Sub btnNew_Click(sender As System.Object, e As System.EventArgs) Handles btnNew.Click

        'get the SupplierID from the ComboBox

        Dim strSuppID As String = cboSuppAll.Text.Substring(0, cboSuppAll.Text.IndexOf(" "))

        'SQL string to get required fields

        Dim strSQL As String = "SELECT tblItem.StockID,Description,ReorderQuantity AS Qty " & "FROM tblItem, tblSupplierItem " & "WHERE tblItem.StockID = tblSupplierItem.StockID " & "AND tblSupplierItem.SupplierID = '" & strSuppID & "'" & "AND QuantityInStock < ReorderLevel "

        'get records from DB into DataTable

        DTNewOrder = New DataTable

        Dim DANewOrder As New OleDbDataAdapter(strSQL, CN)

        DANewOrder.Fill(DTNewOrder)

        'display records in DataGridView

        DGV2.DataSource = DTNewOrder

        'stop it going blue

        DGV2.CurrentCell = Nothing

    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click

        'new row for DGV2

        Dim DRNew As DataRow = DTNewOrder.NewRow

        'get copy of Items row

        Dim DR As DataRow = DTItems(ListBox1.SelectedIndex)

        'copy contents

        DRNew("StockID") = DR.Item("StockID")

        DRNew("Description") = DR.Item("Description")

        DRNew("Qty") = DR.Item("ReorderQuantity")

        DTNewOrder.Rows.Add(DRNew)

    End Sub

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click

        With DGV2

            'Skip if there are no records

            If .RowCount = 0 Then Exit Sub

            'Memorise selected row

            Dim intSelRow = .CurrentRow.Index

            'Remove the selected record

            .Rows.RemoveAt(intSelRow)

            'If it wasn't the last record

            If .RowCount <> 0 Then

                'If it was the end one

                If intSelRow = .RowCount Then

                    'Move the selected row up

                    intSelRow -= 1

                End If

                'Reposition selected row

                .CurrentCell = .Item(0, intSelRow)

                .CurrentCell.Selected = True

            End If

        End With

    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

        SaveOrder()

        SaveOrderItems()

    End Sub

    Private Sub SaveOrder()

        'Get new Order record data from boxes:

        Dim strOrderID As String = txtOrderID.Text

        If strOrderID = "" Then Exit Sub

        Dim strSuppID As String = cboSuppAll.Text.Substring(0, cboSuppAll.Text.IndexOf(" "))

        'adjust the order of the date string

        Dim strdate As String = Format(CDate(txtDate.Text), "MM/dd/yyyy")

        'SQL string to add new record to table:

        Dim strSQL As String = "INSERT INTO tblOrder " & "VALUES('" & strOrderID & "','" & strSuppID & "',#" & strdate & "#,NULL)"

        CN.Open()

        Dim CM As New OleDbCommand(strSQL, CN)

        CM.ExecuteNonQuery()

        CN.Close()

    End Sub

    Private Sub SaveOrderItems()

        'Get new OrderID

        Dim strOrderID As String = txtOrderID.Text

        If strOrderID = "" Then Exit Sub

        Dim strStockID, strSQL As String

        Dim intQty As Integer

        Dim CM As New OleDbCommand

        CN.Open()

        CM.Connection = CN

        'Do all the rows of the order

        For Each Row As DataGridViewRow In DGV2.Rows

            'construct SQL command

            strStockID = Row.Cells("StockID").Value.ToString

            intQty = CInt(Row.Cells("Qty").Value)

            strSQL = "INSERT INTO tblOrderItem " & "VALUES('" & strOrderID & "','" & strStockID & "'," & intQty & ")"

            CM.CommandText = strSQL

            CM.ExecuteNonQuery()

        Next

        CN.Close()

        CM.Dispose()

    End Sub

    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click

        'Get Order data into variables

        Dim strOrderID As String = txtOrderID.Text

        If strOrderID = "" Then Exit Sub

        Dim strSupplier As String = cboSuppAll.Text

        Dim strDate As String = txtDate.Text

        'Line to write to file

        Dim strLine As String = ""

        'Filename using OrderID

        Dim strFileName As String = "Order" & strOrderID & ".txt"

        'Open disk file stream for writing

        Dim SW As New StreamWriter(strFileName)

        'Format and write Order details:

        strLine = "ORDER No.: " & strOrderID.ToString & vbCrLf & "SUPPLIER : " & strSupplier.ToString & vbCrLf & "DATE : " & strDate.ToString & vbCrLf

        SW.WriteLine(strLine)

        'OrderItems headings:

        strLine = "StockID Description Qty"

        SW.WriteLine(strLine)

        'OrderItems:

        For Each Row As DataGridViewRow In DGV2.Rows

            'Build output line

            strLine = Row.Cells("StockID").Value.ToString.PadRight(10) & Row.Cells("Description").Value.ToString.PadRight(20) & Row.Cells("Qty").Value.ToString()

            'Add new OrderItem line to file

            SW.WriteLine(strLine)

        Next

        'Close stream

        SW.Close()

        'Open text file in Notepad

        System.Diagnostics.Process.Start(strFileName)

    End Sub


End Class