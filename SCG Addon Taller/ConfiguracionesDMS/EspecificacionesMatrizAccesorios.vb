Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class EspecificacionesMatrizAccesorios : Inherits MatrixSBO

    Private _ColItemCode As ColumnaMatrixSBOEditText(Of String)
    Private _ColItemName As ColumnaMatrixSBOEditText(Of String)

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Public Property ColItemCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColItemCode
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColItemCode = value
        End Set
    End Property

    Public Property ColItemName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColItemName
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColItemName = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()
        _ColItemCode = New ColumnaMatrixSBOEditText(Of String)("Col_Code", True, "code", Me)
        _ColItemName = New ColumnaMatrixSBOEditText(Of String)("Col_Name", True, "name", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _ColItemCode.AsignaBindingDataTable()
        _ColItemName.AsignaBindingDataTable()
    End Sub
End Class
