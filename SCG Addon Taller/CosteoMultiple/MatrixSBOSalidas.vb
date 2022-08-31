Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOSalidas : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaUnidad As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaUnidad() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaUnidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaUnidad = value
        End Set
    End Property

    Private _columnaMarca As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaMarca() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMarca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMarca = value
        End Set
    End Property

    Private _columnaEstilo As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaEstilo() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaEstilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaEstilo = value
        End Set
    End Property


    Private _columnaVIN As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaVIN() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaVIN
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaVIN = value
        End Set
    End Property


    Private _columnaID As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaID() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaID
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaID = value
        End Set
    End Property


    Private _columnaEntrada As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaEntrada() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaEntrada
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaEntrada = value
        End Set
    End Property

    Private _columnaSeleccionar As ColumnaMatrixSBOCheckBox(Of String)

    Public Property ColumnaSeleccionar() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccionar
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccionar = value
        End Set
    End Property


    Private _columnaGastra As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaGastra() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaGastra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaGastra = value
        End Set
    End Property

    Private _columnaGastra_S As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaGastra_S() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaGastra_S
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaGastra_S = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        _columnaSeleccionar = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "seleccion", Me)
        _columnaEntrada = New ColumnaMatrixSBOEditText(Of String)("col_DocEn", True, "entrada", Me)
        _columnaUnidad = New ColumnaMatrixSBOEditText(Of String)("col_Unid", True, "unidad", Me)
        _columnaMarca = New ColumnaMatrixSBOEditText(Of String)("col_Mar", True, "marca", Me)
        _columnaEstilo = New ColumnaMatrixSBOEditText(Of String)("col_Est", True, "estilo", Me)
        _columnaVIN = New ColumnaMatrixSBOEditText(Of String)("col_VIN", True, "vin", Me)
        _columnaID = New ColumnaMatrixSBOEditText(Of String)("col_ID_V", True, "id", Me)
        _columnaGastra = New ColumnaMatrixSBOEditText(Of Decimal)("col_Gastra", True, "Gastra", Me)
        _columnaGastra_S = New ColumnaMatrixSBOEditText(Of Decimal)("col_GastrS", True, "Gastra_S", Me)


    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccionar.AsignaBindingDataTable()
        _columnaEntrada.AsignaBindingDataTable()
        _columnaUnidad.AsignaBindingDataTable()
        _columnaMarca.AsignaBindingDataTable()
        _columnaEstilo.AsignaBindingDataTable()
        _columnaVIN.AsignaBindingDataTable()
        _columnaID.AsignaBindingDataTable()
        _columnaGastra.AsignaBindingDataTable()
        _columnaGastra_S.AsignaBindingDataTable()

    End Sub



End Class
