Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Public Class MatrizSBOVehiculo : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#Region "Columnas Matriz"


    Public _columnaTipoDocumento As ColumnaMatrixSBO(Of String)

    Public Property TipoDocumento As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaTipoDocumento
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaTipoDocumento = value
        End Set
    End Property

    Public _columnaFechaContabilizacion As ColumnaMatrixSBO(Of Date)

    Public Property FechaContabilizacion As ColumnaMatrixSBO(Of Date)
        Get
            Return _columnaFechaContabilizacion
        End Get
        Set(value As ColumnaMatrixSBO(Of Date))
            _columnaFechaContabilizacion = value
        End Set
    End Property

    Public _columnaDocEntry As ColumnaMatrixSBO(Of String)

    Public Property DocEntry As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaDocEntry
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaDocEntry = value
        End Set
    End Property

    Public _columnaUnidad As ColumnaMatrixSBO(Of String)
    Public Property Unidad As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaUnidad
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaUnidad = value
        End Set
    End Property

    Public _columnaAsiento As ColumnaMatrixSBO(Of String)
    Public Property Asiento As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaAsiento
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaAsiento = value
        End Set
    End Property

    Public _columnaTraslado As ColumnaMatrixSBO(Of String)
    Public Property ColumnaTraslado As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaTraslado
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaTraslado = value
        End Set
    End Property

    Public _columnaEntradaLocal As ColumnaMatrixSBO(Of Double)
    Public Property EntradaLocal As ColumnaMatrixSBO(Of Double)
        Get
            Return _columnaEntradaLocal
        End Get
        Set(value As ColumnaMatrixSBO(Of Double))
            _columnaEntradaLocal = value
        End Set
    End Property

    Public _columnaEntradaSistema As ColumnaMatrixSBO(Of Double)
    Public Property EntradaSistema As ColumnaMatrixSBO(Of Double)
        Get
            Return _columnaEntradaSistema
        End Get
        Set(value As ColumnaMatrixSBO(Of Double))
            _columnaEntradaSistema = value
        End Set
    End Property

    Public _columnaSalidaLocal As ColumnaMatrixSBO(Of Double)
    Public Property SalidaLocal As ColumnaMatrixSBO(Of Double)
        Get
            Return _columnaSalidaLocal
        End Get
        Set(value As ColumnaMatrixSBO(Of Double))
            _columnaSalidaLocal = value
        End Set
    End Property

    Public _columnaSalidaSistema As ColumnaMatrixSBO(Of Double)
    Public Property SalidaSistema As ColumnaMatrixSBO(Of Double)
        Get
            Return _columnaSalidaSistema
        End Get
        Set(value As ColumnaMatrixSBO(Of Double))
            _columnaSalidaSistema = value
        End Set
    End Property

    Public _columnaTipoInventario As ColumnaMatrixSBO(Of String)
    Public Property TipoInventario As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaTipoInventario
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaTipoInventario = value
        End Set
    End Property

    Public _columnaNombreInventario As ColumnaMatrixSBO(Of String)
    Public Property NombreInventario As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaNombreInventario
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaNombreInventario = value
        End Set
    End Property

    Public _columnaIdVehiculo As ColumnaMatrixSBO(Of Integer)
    Public Property IdVehiculo As ColumnaMatrixSBO(Of Integer)
        Get
            Return _columnaIdVehiculo
        End Get
        Set(value As ColumnaMatrixSBO(Of Integer))
            _columnaIdVehiculo = value
        End Set
    End Property


    Public _columnaValorAcumulado As ColumnaMatrixSBO(Of Double)
    Public Property ValorAcumulado As ColumnaMatrixSBO(Of Double)
        Get
            Return _columnaValorAcumulado
        End Get
        Set(value As ColumnaMatrixSBO(Of Double))
            _columnaValorAcumulado = value
        End Set
    End Property

    Public _columnaDescripcionTrasaldo As ColumnaMatrixSBO(Of String)
    Public Property DescripcionTrasaldo As ColumnaMatrixSBO(Of String)
        Get
            Return _columnaDescripcionTrasaldo
        End Get
        Set(value As ColumnaMatrixSBO(Of String))
            _columnaDescripcionTrasaldo = value
        End Set
    End Property

#End Region


    Public Overrides Sub CreaColumnas()

        _columnaTipoDocumento = New ColumnaMatrixSBOCheckBox(Of String)("col_TipoD", True, "TipoDocumento", Me)
        _columnaFechaContabilizacion = New ColumnaMatrixSBOEditText(Of Date)("col_FechC", True, "FechaContabilizacion", Me)
        _columnaDocEntry = New ColumnaMatrixSBOEditText(Of String)("col_DocEn", True, "docentry", Me)
        _columnaUnidad = New ColumnaMatrixSBOEditText(Of String)("col_Uni", True, "Unidad", Me)
        _columnaAsiento = New ColumnaMatrixSBOEditText(Of String)("col_As", True, "Asiento", Me)
        _columnaEntradaLocal = New ColumnaMatrixSBOEditText(Of Double)("col_TEntL", True, "Total_EntradaLocal", Me)
        _columnaEntradaSistema = New ColumnaMatrixSBOEditText(Of Double)("col_TEntS", True, "Total_EntradaSistema", Me)
        _columnaSalidaLocal = New ColumnaMatrixSBOEditText(Of Double)("col_TSalL", True, "Total_SalidaLocal", Me)
        _columnaSalidaSistema = New ColumnaMatrixSBOEditText(Of Double)("col_TSalS", True, "Total_SalidaSistema", Me)
        _columnaTipoInventario = New ColumnaMatrixSBOEditText(Of String)("col_Tipo", True, "Tipo", Me)
        _columnaTraslado = New ColumnaMatrixSBOEditText(Of String)("col_Tras", True, "Trasladado", Me)
        _columnaNombreInventario = New ColumnaMatrixSBOEditText(Of String)("col_NoInv", True, "NombreInventario", Me)
        _columnaIdVehiculo = New ColumnaMatrixSBOEditText(Of Integer)("col_IdVe", True, "IdVehiculo", Me)
        _columnaValorAcumulado = New ColumnaMatrixSBOEditText(Of Double)("col_VaAc", True, "ValorAcumulado", Me)
        _columnaDescripcionTrasaldo = New ColumnaMatrixSBOEditText(Of String)("col_EnTras", True, "DescTraslado", Me)

     
    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaTipoDocumento.AsignaBindingDataTable()
        _columnaFechaContabilizacion.AsignaBindingDataTable()
        _columnaDocEntry.AsignaBindingDataTable()
        _columnaUnidad.AsignaBindingDataTable()
        _columnaAsiento.AsignaBindingDataTable()
        _columnaEntradaLocal.AsignaBindingDataTable()
        _columnaEntradaSistema.AsignaBindingDataTable()
        _columnaSalidaLocal.AsignaBindingDataTable()
        _columnaSalidaSistema.AsignaBindingDataTable()
        _columnaTipoInventario.AsignaBindingDataTable()
        _columnaTraslado.AsignaBindingDataTable()
        _columnaNombreInventario.AsignaBindingDataTable()
        _columnaIdVehiculo.AsignaBindingDataTable()
        _columnaValorAcumulado.AsignaBindingDataTable()
        _columnaDescripcionTrasaldo.AsignaBindingDataTable()

    End Sub


End Class
