
'*******************************************
'*Matriz para manejo de los repuestos
'*******************************************

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizVehiculosCampana
    : Inherits MatrixSBO

#Region "Declaraciones"

    'Columnas de la matriz
    Private _columnaCol_Num As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Uni As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mar As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mod As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Est As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pla As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cli As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Es As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ano As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Vin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ind As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"
    'propiedades para las columnas de la matriz

    Public Property ColumnaColNum As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Num
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Num = value
        End Set
    End Property

    Public Property ColumnaColUni As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Uni
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Uni = value
        End Set
    End Property

    Public Property ColumnaColMar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mar = value
        End Set
    End Property

    Public Property ColumnaColMod As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mod = value
        End Set
    End Property

    Public Property ColumnaColEst As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Est
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Est = value
        End Set
    End Property

    Public Property ColumnaColPla As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Pla
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Pla = value
        End Set
    End Property

    Public Property ColumnaColCli As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cli
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cli = value
        End Set
    End Property

    Public Property ColumnaColEs As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Es
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Es = value
        End Set
    End Property

    Public Property ColumnaColAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ano
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ano = value
        End Set
    End Property

    Public Property ColumnaColVin As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Vin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Vin = value
        End Set
    End Property

    Public Property ColumnaColInd As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ind
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ind = value
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

    ''' <summary>
    ''' Crea las columnas para la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CreaColumnas()

        'ColumnaColNum = New ColumnaMatrixSBOEditText(Of String)("Col_num", True, "num", Me)
        ColumnaColUni = New ColumnaMatrixSBOEditText(Of String)("Col_uni", True, "uni", Me)
        ColumnaColMar = New ColumnaMatrixSBOEditText(Of String)("Col_mar", True, "mar", Me)
        ColumnaColMod = New ColumnaMatrixSBOEditText(Of String)("Col_mod", True, "mod", Me)
        ColumnaColEst = New ColumnaMatrixSBOEditText(Of String)("Col_est", True, "est", Me)
        ColumnaColPla = New ColumnaMatrixSBOEditText(Of String)("Col_pla", True, "pla", Me)
        ColumnaColCli = New ColumnaMatrixSBOEditText(Of String)("Col_cli", True, "cli", Me)
        ColumnaColEs = New ColumnaMatrixSBOEditText(Of String)("Col_es", True, "es", Me)
        ColumnaColAno = New ColumnaMatrixSBOEditText(Of String)("Col_ano", True, "ano", Me)
        ColumnaColVin = New ColumnaMatrixSBOEditText(Of String)("Col_vin", True, "vin", Me)

    End Sub

    ''' <summary>
    ''' Liga las columnas de la matriz
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub LigaColumnas()

        'ColumnaColNum.AsignaBindingDataTable()
        ColumnaColUni.AsignaBindingDataTable()
        ColumnaColMar.AsignaBindingDataTable()
        ColumnaColMod.AsignaBindingDataTable()
        ColumnaColEst.AsignaBindingDataTable()
        ColumnaColPla.AsignaBindingDataTable()
        ColumnaColCli.AsignaBindingDataTable()
        ColumnaColEs.AsignaBindingDataTable()
        ColumnaColAno.AsignaBindingDataTable()
        ColumnaColVin.AsignaBindingDataTable()

    End Sub

#End Region

End Class

