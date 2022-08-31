Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Public Class MatrizEntradaPedido : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaSel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaPed As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCodArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDesArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCodCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCanR As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCanS As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCost As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaImp As ColumnaMatrixSBOEditText(Of String)
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

    Public Property ColumnaPed As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPed
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPed = value
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

    Public Property ColumnaCanR As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCanR
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCanR = value
        End Set
    End Property

    Public Property ColumnaCanS As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCanS
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCanS = value
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

    Public Property ColumnaTot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaTot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaTot = value
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

    Public Property ColumnaRef As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaRef
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaRef = value
        End Set
    End Property
#End Region

    Public Overrides Sub CreaColumnas()
        'ColumnaSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "sel", Me)

        ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Pedi", True, "U_Num_Ped", Me)
        ColumnaCodArt = New ColumnaMatrixSBOEditText(Of String)("col_Code", True, "U_Cod_Art", Me)
        ColumnaDesArt = New ColumnaMatrixSBOEditText(Of String)("col_Desc", True, "U_Desc_Art", Me)
        ColumnaCodCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "U_Cod_Col", Me)
        ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "U_Ano_Veh", Me)
        ColumnaCanR = New ColumnaMatrixSBOEditText(Of String)("col_Cant", True, "U_Cant_Ent", Me)
        ColumnaCanS = New ColumnaMatrixSBOEditText(Of String)("col_Solic", True, "U_Cant_Veh", Me)
        ColumnaCost = New ColumnaMatrixSBOEditText(Of String)("col_Cost", True, "U_Mnt_Linea", Me)
        ColumnaTot = New ColumnaMatrixSBOEditText(Of String)("col_Total", True, "U_Total_L", Me)
        ColumnaRef = New ColumnaMatrixSBOEditText(Of String)("col_Ref", True, "U_Line_Ref", Me)

        'ColumnaImp = New ColumnaMatrixSBOEditText(Of String)("col_Imp", True, "U_Cod_Imp", Me)
        'ColumnaRef = New ColumnaMatrixSBOEditText(Of String)("col_Ref", True, "U_Line_Ref", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

    End Sub

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

End Class


