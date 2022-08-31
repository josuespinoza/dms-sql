Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizConsultaInventario : Inherits MatrixSBO

#Region "Declaraciones"

    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Unid As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Vin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Mot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Marca As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Estilo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Modelo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_MarcaMot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_DiasInv As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Transmision As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Traccion As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Combustible As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Techo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Ubicacion As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Tipo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Disponibilidad As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_ColorInt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_ColorExt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Carroceria As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cabina As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Categoria As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Año As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Estado As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_FecArr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_FecRes As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_FecVen As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Vendedor As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Moneda As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_PrecioVenta As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_ValorNeto As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Placa As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Reserva As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_CardName As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Bono As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaColCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Code
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Code = value
        End Set
    End Property

    Public Property ColumnaColUnidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Unid
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Unid = value
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

    Public Property ColumnaColMot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mot = value
        End Set
    End Property
    Public Property ColumnaColMarca As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Marca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Marca = value
        End Set
    End Property

    Public Property ColumnaColEstilo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Estilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Estilo = value
        End Set
    End Property

    Public Property ColumnaColModelo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Modelo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Modelo = value
        End Set
    End Property

    Public Property ColumnaColMarcaMot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_MarcaMot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_MarcaMot = value
        End Set
    End Property

    Public Property ColumnaColDiasInv As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DiasInv
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DiasInv = value
        End Set
    End Property

    Public Property ColumnaColTransmision As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Transmision
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Transmision = value
        End Set
    End Property

    Public Property ColumnaColTraccion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Traccion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Traccion = value
        End Set
    End Property

    Public Property ColumnaColCombustible As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Combustible
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Combustible = value
        End Set
    End Property

    Public Property ColumnaColTecho As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Techo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Techo = value
        End Set
    End Property

    Public Property ColumnaColUbicacion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Ubicacion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Ubicacion = value
        End Set
    End Property

    Public Property ColumnaColTipo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tipo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tipo = value
        End Set
    End Property

    Public Property ColumnaColDisponibilidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Disponibilidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Disponibilidad = value
        End Set
    End Property

    Public Property ColumnaColColorInt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ColorInt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ColorInt = value
        End Set
    End Property

    Public Property ColumnaColColorExt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ColorExt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ColorExt = value
        End Set
    End Property

    Public Property ColumnaColCarrocieria As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Carroceria
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Carroceria = value
        End Set
    End Property

    Public Property ColumnaColCabina As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cabina
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cabina = value
        End Set
    End Property

    Public Property ColumnaColCategoria As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Categoria
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Categoria = value
        End Set
    End Property

    Public Property ColumnaColAño As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Año
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Año = value
        End Set
    End Property

    Public Property ColumnaColEstado As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Estado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Estado = value
        End Set
    End Property

    Public Property ColumnaColFechaArr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_FecArr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_FecArr = value
        End Set
    End Property

    Public Property ColumnaColFechaReserva As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_FecRes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_FecRes = value
        End Set
    End Property

    Public Property ColumnaColFechaVenRes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_FecVen
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_FecVen = value
        End Set
    End Property

    Public Property ColumnaColVendedor As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Vendedor
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Vendedor = value
        End Set
    End Property

    Public Property ColumnaColMoneda As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Moneda
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Moneda = value
        End Set
    End Property

    Public Property ColumnaColPrecioVenta As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_PrecioVenta
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_PrecioVenta = value
        End Set
    End Property

    Public Property ColumnaColValorNeto As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ValorNeto
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ValorNeto = value
        End Set
    End Property

    Public Property ColumnaColPlaca As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Placa
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Placa = value
        End Set
    End Property

    Public Property ColumnaColReserva As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Reserva
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Reserva = value
        End Set
    End Property

    Public Property ColumnaCardName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_CardName
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_CardName = value
        End Set
    End Property


    Public Property ColumnaColBono As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Bono
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Bono = value
        End Set
    End Property


#End Region

#Region "Constructor"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Métodos"

    Public Overrides Sub CreaColumnas()
        ColumnaColCode = New ColumnaMatrixSBOEditText(Of String)("Col_Code", True, "Col_Code", Me)
        ColumnaColUnidad = New ColumnaMatrixSBOEditText(Of String)("Col_Unid", True, "Col_Unid", Me)
        ColumnaColVin = New ColumnaMatrixSBOEditText(Of String)("Col_Vin", True, "Col_Vin", Me)
        ColumnaColMot = New ColumnaMatrixSBOEditText(Of String)("Col_Mot", True, "Col_Mot", Me)
        ColumnaColMarca = New ColumnaMatrixSBOEditText(Of String)("Col_Marca", True, "Col_Marca", Me)
        ColumnaColEstilo = New ColumnaMatrixSBOEditText(Of String)("Col_Estilo", True, "Col_Estilo", Me)
        ColumnaColModelo = New ColumnaMatrixSBOEditText(Of String)("Col_Mode", True, "Col_Mode", Me)
        ColumnaCardName = New ColumnaMatrixSBOEditText(Of String)("CardName", True, "CardName", Me)
        ColumnaColDiasInv = New ColumnaMatrixSBOEditText(Of String)("Col_Dias", True, "Col_Dias", Me)
        ColumnaColMarcaMot = New ColumnaMatrixSBOEditText(Of String)("Col_MarcM", True, "Col_MarcM", Me)
        ColumnaColTransmision = New ColumnaMatrixSBOEditText(Of String)("Col_Trans", True, "Col_Trans", Me)
        ColumnaColTraccion = New ColumnaMatrixSBOEditText(Of String)("Col_Tracc", True, "Col_Tracc", Me)
        ColumnaColCombustible = New ColumnaMatrixSBOEditText(Of String)("Col_Combu", True, "Col_Combu", Me)
        ColumnaColTecho = New ColumnaMatrixSBOEditText(Of String)("Col_Tech", True, "Col_Tech", Me)
        ColumnaColUbicacion = New ColumnaMatrixSBOEditText(Of String)("Col_Ubic", True, "Col_Ubic", Me)
        ColumnaColTipo = New ColumnaMatrixSBOEditText(Of String)("Col_Tipo", True, "Col_Tipo", Me)
        ColumnaColDisponibilidad = New ColumnaMatrixSBOEditText(Of String)("Col_Dispo", True, "Col_Dispo", Me)
        ColumnaColColorExt = New ColumnaMatrixSBOEditText(Of String)("Col_Col", True, "Col_Col", Me)
        ColumnaColColorInt = New ColumnaMatrixSBOEditText(Of String)("Col_ColTa", True, "Col_ColTa", Me)
        ColumnaColCarrocieria = New ColumnaMatrixSBOEditText(Of String)("Col_Carro", True, "Col_Carro", Me)
        ColumnaColCabina = New ColumnaMatrixSBOEditText(Of String)("Col_Cab", True, "Col_Cab", Me)
        ColumnaColCategoria = New ColumnaMatrixSBOEditText(Of String)("Col_Cate", True, "Col_Cate", Me)
        ColumnaColAño = New ColumnaMatrixSBOEditText(Of String)("Col_Ano", True, "Col_Ano", Me)
        ColumnaColEstado = New ColumnaMatrixSBOEditText(Of String)("Col_Esta", True, "Col_Esta", Me)
        ColumnaColFechaArr = New ColumnaMatrixSBOEditText(Of String)("Col_FecAr", True, "Col_FecAr", Me)
        ColumnaColFechaReserva = New ColumnaMatrixSBOEditText(Of String)("Col_FecRe", True, "Col_FecRe", Me)
        ColumnaColFechaVenRes = New ColumnaMatrixSBOEditText(Of String)("Col_FecVe", True, "Col_FecVe", Me)
        ColumnaColVendedor = New ColumnaMatrixSBOEditText(Of String)("Col_Vend", True, "Col_Vend", Me)
        ColumnaColMoneda = New ColumnaMatrixSBOEditText(Of String)("Col_Mon", True, "Col_Mon", Me)
        ColumnaColPrecioVenta = New ColumnaMatrixSBOEditText(Of String)("Col_Pre", True, "Col_Pre", Me)
        ColumnaColValorNeto = New ColumnaMatrixSBOEditText(Of String)("Col_Val", True, "Col_Val", Me)
        ColumnaColBono = New ColumnaMatrixSBOEditText(Of String)("Col_Bon", True, "Col_Bon", Me)
        ColumnaColPlaca = New ColumnaMatrixSBOEditText(Of String)("col_Plac", True, "col_Plac", Me)
        ColumnaColReserva = New ColumnaMatrixSBOEditText(Of String)("col_Res", True, "col_Res", Me)
    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaColCode.AsignaBindingDataTable()
        ColumnaColUnidad.AsignaBindingDataTable()
        ColumnaColVin.AsignaBindingDataTable()
        ColumnaColMot.AsignaBindingDataTable()
        ColumnaColMarca.AsignaBindingDataTable()
        ColumnaColEstilo.AsignaBindingDataTable()
        ColumnaColModelo.AsignaBindingDataTable()
        ColumnaCardName.AsignaBindingDataTable()
        ColumnaColDiasInv.AsignaBindingDataTable()
        ColumnaColMarcaMot.AsignaBindingDataTable()
        ColumnaColTransmision.AsignaBindingDataTable()
        ColumnaColTraccion.AsignaBindingDataTable()
        ColumnaColCombustible.AsignaBindingDataTable()
        ColumnaColTecho.AsignaBindingDataTable()
        ColumnaColUbicacion.AsignaBindingDataTable()
        ColumnaColTipo.AsignaBindingDataTable()
        ColumnaColDisponibilidad.AsignaBindingDataTable()
        ColumnaColColorExt.AsignaBindingDataTable()
        ColumnaColColorInt.AsignaBindingDataTable()
        ColumnaColCarrocieria.AsignaBindingDataTable()
        ColumnaColCabina.AsignaBindingDataTable()
        ColumnaColCategoria.AsignaBindingDataTable()
        ColumnaColAño.AsignaBindingDataTable()
        ColumnaColEstado.AsignaBindingDataTable()
        ColumnaColFechaArr.AsignaBindingDataTable()
        ColumnaColFechaReserva.AsignaBindingDataTable()
        ColumnaColFechaVenRes.AsignaBindingDataTable()
        ColumnaColVendedor.AsignaBindingDataTable()
        ColumnaColMoneda.AsignaBindingDataTable()
        ColumnaColPrecioVenta.AsignaBindingDataTable()
        ColumnaColValorNeto.AsignaBindingDataTable()
        ColumnaColBono.AsignaBindingDataTable()
        ColumnaColPlaca.AsignaBindingDataTable()
        ColumnaColFechaArr.AsignaBindingDataTable()
        ColumnaColReserva.AsignaBindingDataTable()
    End Sub

#End Region

End Class
