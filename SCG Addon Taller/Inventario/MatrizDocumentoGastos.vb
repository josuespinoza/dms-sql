
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizDocumentoGastos : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaCol_sel As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Can As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mon As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pre As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cos As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Imp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Lnum As ColumnaMatrixSBOEditText(Of String)
#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

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
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
        End Set
    End Property

    Public Property ColumnaColDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    Public Property ColumnaColCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Can
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Can = value
        End Set
    End Property

    Public Property ColumnaColMon As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mon = value
        End Set
    End Property

    Public Property ColumnaColPre As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Pre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Pre = value
        End Set
    End Property

    Public Property ColumnaColCos As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cos = value
        End Set
    End Property

    Public Property ColumnaColImp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Imp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Imp = value
        End Set
    End Property

    Public Property ColumnaColLnum As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Lnum
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Lnum = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Metodos"

    Public Overrides Sub CreaColumnas()
        ColumnaColSel = New ColumnaMatrixSBOEditText(Of String)("Col_sel", True, "sel", Me)
        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
        ColumnaColCan = New ColumnaMatrixSBOEditText(Of String)("Col_can", True, "can", Me)
        ColumnaColMon = New ColumnaMatrixSBOEditText(Of String)("Col_mon", True, "mon", Me)
        ColumnaColCos = New ColumnaMatrixSBOEditText(Of String)("Col_cos", True, "cos", Me)
        ColumnaColPre = New ColumnaMatrixSBOEditText(Of String)("Col_pre", True, "pre", Me)
        ColumnaColImp = New ColumnaMatrixSBOEditText(Of String)("Col_imp", True, "imp", Me)
        ColumnaColLnum = New ColumnaMatrixSBOEditText(Of String)("Col_lnum", True, "lnum", Me)

    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColCan.AsignaBindingDataTable()
        ColumnaColMon.AsignaBindingDataTable()
        ColumnaColPre.AsignaBindingDataTable()
        ColumnaColCos.AsignaBindingDataTable()
        ColumnaColImp.AsignaBindingDataTable()
        ColumnaColLnum.AsignaBindingDataTable()

    End Sub

#End Region


End Class


