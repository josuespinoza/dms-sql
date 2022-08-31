
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizPedidoDeVehiculos : Inherits MatrixSBO

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Declaraciones"

    Private _columnaSel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDes As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCan As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCos As ColumnaMatrixSBOEditText(Of String)
    Private _columnaTot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaRec As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPen As ColumnaMatrixSBOEditText(Of String)
    Private _columnaLin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCer As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSel = value
        End Set
    End Property

    Public Property ColumnaCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCod = value
        End Set
    End Property

    Public Property ColumnaDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDes = value
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

    Public Property ColumnaCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol = value
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

    Public Property ColumnaCos As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCos = value
        End Set
    End Property

    Public Property ColumnaTot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaTot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnatot = value
        End Set
    End Property

    Public Property ColumnaRec As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaRec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaRec = value
        End Set
    End Property

    Public Property ColumnaPen As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPen
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPen = value
        End Set
    End Property

    Public Property ColumnaLin As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaLin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaLin = value
        End Set
    End Property

    Public Property ColumnaCer As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCer
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCer = value
        End Set
    End Property



#End Region



    Public Overrides Sub CreaColumnas()

        'ColumnaSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "sel", Me)
        ColumnaCod = New ColumnaMatrixSBOEditText(Of String)("col_Cod", True, "U_Cod_Art", Me)
        ColumnaDes = New ColumnaMatrixSBOEditText(Of String)("col_Des", True, "U_Desc_Art", Me)
        ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "U_Ano_Veh", Me)
        ColumnaCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "U_Cod_Col", Me)
        ColumnaCan = New ColumnaMatrixSBOEditText(Of String)("col_Can", True, "U_Cant", Me)
        ColumnaCos = New ColumnaMatrixSBOEditText(Of String)("col_Cos", True, "U_Cost_Art", Me)
        ColumnaTot = New ColumnaMatrixSBOEditText(Of String)("col_Tot", True, "U_Cost_Tot", Me)
        ColumnaRec = New ColumnaMatrixSBOEditText(Of String)("col_Rec", True, "U_Cant_Rec", Me)
        ColumnaPen = New ColumnaMatrixSBOEditText(Of String)("col_Pen", True, "U_Pen_Rec", Me)
        ColumnaLin = New ColumnaMatrixSBOEditText(Of String)("col_Lin", True, "LineId", Me)
        ColumnaCer = New ColumnaMatrixSBOEditText(Of String)("col_Cer", True, "U_Cerrada", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

    End Sub


End Class


