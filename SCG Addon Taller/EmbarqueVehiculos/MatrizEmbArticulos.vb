Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizEmbArticulos
    : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaColCod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColDes As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColCan As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

    Public Property ColumnaColCod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColCod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColCod = value
        End Set
    End Property

    Public Property ColumnaColDes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColDes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColDes = value
        End Set
    End Property

    Public Property ColumnaColCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColCol = value
        End Set
    End Property

    Public Property ColumnaColCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColCan
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColCan = value
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

#Region "Metodos"

    Public Overrides Sub LigaColumnas()

        ColumnaColCod.AsignaBindingDataTable()
        ColumnaColDes.AsignaBindingDataTable()
        ColumnaColCol.AsignaBindingDataTable()
        ColumnaColCan.AsignaBindingDataTable()

    End Sub

    Public Overrides Sub CreaColumnas()

        ColumnaColCod = New ColumnaMatrixSBOEditText(Of String)("ColCod", True, "cod", Me)
        ColumnaColDes = New ColumnaMatrixSBOEditText(Of String)("ColDes", True, "des", Me)
        ColumnaColCol = New ColumnaMatrixSBOEditText(Of String)("ColCol", True, "col", Me)
        ColumnaColCan = New ColumnaMatrixSBOEditText(Of String)("ColCan", True, "can", Me)

    End Sub

#End Region

End Class
