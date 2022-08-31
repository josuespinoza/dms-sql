Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizArticulosPedidos : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaCodArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDesArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCodCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaDesCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCan As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCost As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCostTot As ColumnaMatrixSBOEditText(Of String)



#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

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

    Public Property ColumnaCostTot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCostTot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCostTot = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="UniqueId">Nombre de la matriz</param>
    ''' <param name="formularioSBO">Objeto formulario</param>
    ''' <param name="tablaLigada">Tabla ligada</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

    Public Overrides Sub CreaColumnas()

        ColumnaCodArt = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaDesArt = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaCodCol = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaDesCol = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaCan = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaCost = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaCostTot = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaCodArt.AsignaBinding()
        ColumnaDesArt.AsignaBinding()
        ColumnaAno.AsignaBinding()
        ColumnaCodCol.AsignaBinding()
        ColumnaDesCol.AsignaBinding()
        ColumnaCan.AsignaBinding()
        ColumnaCost.AsignaBinding()
        ColumnaCostTot.AsignaBinding()

    End Sub

End Class
