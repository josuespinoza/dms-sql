
'*******************************************
'*Matriz para manejo de los repuestos
'*******************************************

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSeleccionaRepuestosOT
    : Inherits MatrixSBO

#Region "Declaraciones"

    Private _columnaCol_sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_bod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_stk As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_can As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_pre As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_apr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_tra As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_mon As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_CodBar As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaColSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
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

    Public Property ColumnaColBod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_bod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_bod = value
        End Set
    End Property

    Public Property ColumnaColStk As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_stk
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_stk = value
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

    Public Property ColumnaColApr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_apr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_apr = value
        End Set
    End Property

    Public Property ColumnaColTra As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_tra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_tra = value
        End Set
    End Property

    Public Property ColumnaColMon As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_mon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_mon = value
        End Set
    End Property

    Public Property ColumnaColCodBar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_CodBar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_CodBar = value
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
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("Col_sel", True, "sel", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
        ColumnaColBod = New ColumnaMatrixSBOEditText(Of String)("Col_bod", True, "bod", Me)
        ColumnaColStk = New ColumnaMatrixSBOEditText(Of String)("Col_onH", True, "onH", Me)
        ColumnaColCan = New ColumnaMatrixSBOEditText(Of String)("Col_can", True, "can", Me)
        ColumnaColPre = New ColumnaMatrixSBOEditText(Of String)("Col_pre", True, "pre", Me)
        ColumnaColMon = New ColumnaMatrixSBOEditText(Of String)("Col_mon", True, "mon", Me)
        ColumnaColCodBar = New ColumnaMatrixSBOEditText(Of String)("Col_CodBar", True, "CodBar", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColBod.AsignaBindingDataTable()
        ColumnaColStk.AsignaBindingDataTable()
        ColumnaColCan.AsignaBindingDataTable()
        ColumnaColPre.AsignaBindingDataTable()
        ColumnaColMon.AsignaBindingDataTable()
        ColumnaColCodBar.AsignaBindingDataTable()
    End Sub

#End Region

End Class
