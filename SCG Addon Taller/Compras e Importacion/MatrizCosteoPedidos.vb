Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizCosteoPedidos : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaSel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCodArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDesArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCodCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDesCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCan As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCost As ColumnaMatrixSBOEditText(Of String)
    Private _columnaImp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPed As ColumnaMatrixSBOEditText(Of String)
    Private _columnaEnt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTImp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaRef As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    'propiedades para las columnas de la matriz

    Public Property ColumnaSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSel = value
        End Set
    End Property

    Public Property ColumnaCodArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodArt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodArt = value
        End Set
    End Property

    Public Property ColumnaDesArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDesArt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDesArt = value
        End Set
    End Property

    Public Property ColumnaAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaAno
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaAno = value
        End Set
    End Property

    Public Property ColumnaCodCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodCol = value
        End Set
    End Property

    Public Property ColumnaDesCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDesCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDesCol = value
        End Set
    End Property

    Public Property ColumnaCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCan
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCan = value
        End Set
    End Property

    Public Property ColumnaCost As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCost
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCost = value
        End Set
    End Property

    Public Property ColumnaPed As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPed
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPed = value
        End Set
    End Property

    Public Property ColumnaImp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaImp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaImp = value
        End Set
    End Property

    Public Property ColumnaEnt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaEnt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaEnt = value
        End Set
    End Property

    Public Property ColumnaTImp As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaTImp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaTImp = value
        End Set
    End Property

    Public Property ColumnaRef As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaRef
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaRef = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub
#End Region

    Public Overrides Sub CreaColumnas()
        'ColumnaSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "sel", Me)
        ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Ped", True, "U_Cod_Pedido", Me)
        ColumnaEnt = New ColumnaMatrixSBOEditText(Of String)("col_Ent", True, "U_Cod_Entrada", Me)
        ColumnaCodArt = New ColumnaMatrixSBOEditText(Of String)("col_Cod", True, "U_Cod_Art", Me)
        ColumnaDesArt = New ColumnaMatrixSBOEditText(Of String)("col_Des", True, "U_Nam_Art", Me)
        ColumnaCodCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "U_Cod_Color", Me)
        ColumnaCan = New ColumnaMatrixSBOEditText(Of String)("col_Can", True, "U_Cant", Me)
        ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "U_Ano_Veh", Me)
        ColumnaCost = New ColumnaMatrixSBOEditText(Of String)("col_Tot", True, "U_Mnt_Linea", Me)
        ColumnaTImp = New ColumnaMatrixSBOEditText(Of String)("col_TImp", True, "U_Mnt_Impuesto", Me)
        ColumnaImp = New ColumnaMatrixSBOEditText(Of String)("col_Imp", True, "U_Cod_Imp", Me)
        ColumnaRef = New ColumnaMatrixSBOEditText(Of String)("col_Ref", True, "U_Line_Ref", Me)
    End Sub

    Public Overrides Sub LigaColumnas()

    End Sub


End Class


