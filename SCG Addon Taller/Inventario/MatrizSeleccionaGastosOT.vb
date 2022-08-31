Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSeleccionaGastosOT
    : Inherits MatrixSBO

#Region "Declaraciones"
    Private _columnaCol_sel As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_can As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_pre As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"


    Public Property ColumnaColSel As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_sel = value
        End Set
    End Property

    Public Property ColumnaColCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_cod = value
        End Set
    End Property

    Public Property ColumnaColDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_des = value
        End Set
    End Property

    
    Public Property ColumnaColCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_can
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_can = value
        End Set
    End Property

    Public Property ColumnaColPre As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_pre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_pre = value
        End Set
    End Property


#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub CreaColumnas()
        ColumnaColSel = New ColumnaMatrixSBOEditText(Of String)("Col_sel", True, "sel", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
        ColumnaColPre = New ColumnaMatrixSBOEditText(Of String)("Col_pre", True, "pre", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColPre.AsignaBindingDataTable()
    End Sub

#End Region

End Class


