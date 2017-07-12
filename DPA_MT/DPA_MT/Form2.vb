Imports System.Data.OleDb

Public Class Form2

    'Database interface objects

    Private CN As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Stock2.mdb;")

    'Moves data between program and database

    Private DA As OleDbDataAdapter

    'Data structure to store records from the DB

    Private DT As New DataTable

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Set up properties of DataGridView

        InitDGV()

        'Get records from DB

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

    'Get records from DB

    Private Sub ReadDB()

        'Give SELECT query string to DataAdapter

        DA = New OleDbDataAdapter("SELECT * FROM tblSupplier", CN)

        'Fill DataTable

        DA.Fill(DT)

        'Bind DataTable to DataGridView

        DGV.DataSource = DT

    End Sub

    'Copy contents of DataGridView row to TextBoxes when row is entered

    Private Sub DGV_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DGV.RowEnter

        txtSupplierID.Text = DGV.Item("SupplierID", e.RowIndex).Value.ToString

        txtName.Text = DGV.Item("Name", e.RowIndex).Value.ToString

        txtTelephone.Text = DGV.Item("Telephone", e.RowIndex).Value.ToString

        txtEmail.Text = DGV.Item("Email", e.RowIndex).Value.ToString

    End Sub

    'Add new record to DataTable using fields from TextBoxes

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        'Create new row for DataTable

        Dim DR As DataRow = DT.NewRow

        'Fill fields from TextBoxes

        DR("SupplierID") = txtSupplierID.Text

        DR("Name") = txtName.Text

        DR("Telephone") = txtTelephone.Text

        DR("Email") = txtEmail.Text

        'Add row to DataTable

        DT.Rows.Add(DR)

        'Select just added record

        DGV.CurrentCell = DGV.Item(0, DGV.RowCount - 1)

    End Sub

    'Amend contents of selected row of DataGridView with contents of TextBoxes

    Private Sub btnAmend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAmend.Click

        DGV.Item("SupplierID", DGV.CurrentRow.Index).Value = txtSupplierID.Text

        DGV.Item("Name", DGV.CurrentRow.Index).Value = txtName.Text

        DGV.Item("Telephone", DGV.CurrentRow.Index).Value = txtTelephone.Text

        DGV.Item("Email", DGV.CurrentRow.Index).Value = txtEmail.Text

        'Write changes into DataTable

        DGV.BindingContext(DT).EndCurrentEdit()

    End Sub

    'Save changes to DataBase

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim CB As OleDbCommandBuilder = New OleDbCommandBuilder(DA)

        DA.Update(DT)

    End Sub

End Class