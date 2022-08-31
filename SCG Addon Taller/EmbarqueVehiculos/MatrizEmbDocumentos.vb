Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizEmbDocumentos
    : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaColNoD As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColDoc As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColFDoc As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

    Public Property ColumnaColNoD As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColNoD
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColNoD = value
        End Set
    End Property

    Public Property ColumnaColDoc As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColDoc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColDoc = value
        End Set
    End Property

    Public Property ColumnaColFDoc As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColFDoc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColFDoc = value
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
        ColumnaColNoD.AsignaBindingDataTable()
        ColumnaColDoc.AsignaBindingDataTable()
        ColumnaColFDoc.AsignaBindingDataTable()
    End Sub

    Public Overrides Sub CreaColumnas()
        ColumnaColNoD = New ColumnaMatrixSBOEditText(Of String)("ColNoD", True, "nod", Me)
        ColumnaColDoc = New ColumnaMatrixSBOEditText(Of String)("ColDoc", True, "doc", Me)
        ColumnaColFDoc = New ColumnaMatrixSBOEditText(Of String)("ColFDoc", True, "fdoc", Me)
    End Sub

#End Region

End Class
