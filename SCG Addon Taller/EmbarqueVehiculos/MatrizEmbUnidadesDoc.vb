Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizEmbUnidadesDoc
    : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaColUni As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColVIN As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColCos As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColMar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColEst As ColumnaMatrixSBOEditText(Of String)
    Private _columnaColMod As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz
    Public Property ColumnaColUni As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColUni
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColUni = value
        End Set
    End Property

    Public Property ColumnaColVIN As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColVIN
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColVIN = value
        End Set
    End Property

    Public Property ColumnaColCos As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColCos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColCos = value
        End Set
    End Property

    Public Property ColumnaColMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColMar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColMar = value
        End Set
    End Property

    Public Property ColumnaColEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColEst
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColEst = value
        End Set
    End Property

    Public Property ColumnaColMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaColMod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaColMod = value
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
        ColumnaColUni.AsignaBindingDataTable()
        ColumnaColVIN.AsignaBindingDataTable()
        ColumnaColCos.AsignaBindingDataTable()
        ColumnaColMar.AsignaBindingDataTable()
        ColumnaColEst.AsignaBindingDataTable()
        ColumnaColMod.AsignaBindingDataTable()
    End Sub

    Public Overrides Sub CreaColumnas()
        ColumnaColUni = New ColumnaMatrixSBOEditText(Of String)("ColUni", True, "uni", Me)
        ColumnaColVIN = New ColumnaMatrixSBOEditText(Of String)("ColVIN", True, "vin", Me)
        ColumnaColCos = New ColumnaMatrixSBOEditText(Of String)("ColCos", True, "cos", Me)
        ColumnaColMar = New ColumnaMatrixSBOEditText(Of String)("ColMar", True, "mar", Me)
        ColumnaColEst = New ColumnaMatrixSBOEditText(Of String)("ColEst", True, "est", Me)
        ColumnaColMod = New ColumnaMatrixSBOEditText(Of String)("ColMod", True, "mod", Me)
    End Sub

#End Region

End Class
