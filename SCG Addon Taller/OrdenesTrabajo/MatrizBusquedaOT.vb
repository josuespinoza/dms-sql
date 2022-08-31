Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizBusquedaOT : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _ColDocEntry As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoOT As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoOTS As ColumnaMatrixSBOEditText(Of String)
    Private _ColTipoOT As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoUnidad As ColumnaMatrixSBOEditText(Of String)
    Private _ColPlaca As ColumnaMatrixSBOEditText(Of String)
    Private _ColEstado As ColumnaMatrixSBOEditText(Of String)
    Private _ColCono As ColumnaMatrixSBOEditText(Of String)
    Private _ColVisita As ColumnaMatrixSBOEditText(Of String)
    'Private _ColDocCita As ColumnaMatrixSBOEditText(Of String)
    'Private _ColCita As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodCliente As ColumnaMatrixSBOEditText(Of String)
    Private _ColNombreCliente As ColumnaMatrixSBOEditText(Of String)
    Private _ColMarca As ColumnaMatrixSBOEditText(Of String)
    Private _ColEstilo As ColumnaMatrixSBOEditText(Of String)
    Private _ColAsesor As ColumnaMatrixSBOEditText(Of String)
    Private _ColFApertura As ColumnaMatrixSBOEditText(Of String)
    Private _ColFProceso As ColumnaMatrixSBOEditText(Of String)
    Private _ColFCierre As ColumnaMatrixSBOEditText(Of String)
    Private _ColSucursal As ColumnaMatrixSBOEditText(Of String)

    Public Property ColDocEntry As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColDocEntry
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColDocEntry = value
        End Set
    End Property

    Public Property ColNoOt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColNoOT
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColNoOT = value
        End Set
    End Property

    Public Property ColNoOtS As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColNoOTS
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColNoOTS = value
        End Set
    End Property

    Public Property ColTipoOt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColTipoOT
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColTipoOT = value
        End Set
    End Property

    Public Property ColNoUnidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColNoUnidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColNoUnidad = value
        End Set
    End Property

    Public Property ColPlaca As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColPlaca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColPlaca = value
        End Set
    End Property

    Public Property ColEstado As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColEstado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColEstado = value
        End Set
    End Property

    Public Property ColCono As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCono
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCono = value
        End Set
    End Property

    Public Property ColCodCliente As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCodCliente
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCodCliente = value
        End Set
    End Property

    Public Property ColNombreCliente As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColNombreCliente
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColNombreCliente = value
        End Set
    End Property

    Public Property ColMarca As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColMarca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColMarca = value
        End Set
    End Property

    Public Property ColEstilo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColEstilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColEstilo = value
        End Set
    End Property

    Public Property ColAsesor As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColAsesor
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColAsesor = value
        End Set
    End Property

    Public Property ColFApertura As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColFApertura
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColFApertura = value
        End Set
    End Property

    Public Property ColFProceso As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColFProceso
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColFProceso = value
        End Set
    End Property

    Public Property ColFCierre As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColFCierre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColFCierre = value
        End Set
    End Property
    
    Public Property ColVisita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColVisita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColVisita = value
        End Set
    End Property

    Public Property ColSucursal As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColSucursal
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColSucursal = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()
        _ColDocEntry = New ColumnaMatrixSBOEditText(Of String)("ColDocE", True, "docentry", Me)
        _ColNoOT = New ColumnaMatrixSBOEditText(Of String)("ColNoOT", True, "noot", Me)
        _ColNoOTS = New ColumnaMatrixSBOEditText(Of String)("ColNoOTS", True, "no_ot", Me)
        _ColTipoOT = New ColumnaMatrixSBOEditText(Of String)("ColTipOT", True, "tipot", Me)
        _ColNoUnidad = New ColumnaMatrixSBOEditText(Of String)("ColNoUni", True, "nouni", Me)
        _ColPlaca = New ColumnaMatrixSBOEditText(Of String)("ColPlaca", True, "placa", Me)
        _ColEstado = New ColumnaMatrixSBOEditText(Of String)("ColEst", True, "est", Me)
        _ColCono = New ColumnaMatrixSBOEditText(Of String)("ColCono", True, "cono", Me)
        _ColVisita = New ColumnaMatrixSBOEditText(Of String)("ColVisi", True, "visita", Me)
        _ColCodCliente = New ColumnaMatrixSBOEditText(Of String)("ColCodCli", True, "codcli", Me)
        _ColNombreCliente = New ColumnaMatrixSBOEditText(Of String)("ColNomCl", True, "nomcl", Me)
        _ColMarca = New ColumnaMatrixSBOEditText(Of String)("ColMar", True, "mar", Me)
        _ColEstilo = New ColumnaMatrixSBOEditText(Of String)("ColEsti", True, "esti", Me)
        _ColAsesor = New ColumnaMatrixSBOEditText(Of String)("ColAsesor", True, "asesor", Me)
        _ColFApertura = New ColumnaMatrixSBOEditText(Of String)("ColFApe", True, "fape", Me)
        _ColFProceso = New ColumnaMatrixSBOEditText(Of String)("ColFPro", True, "fpro", Me)
        _ColFCierre = New ColumnaMatrixSBOEditText(Of String)("ColFCier", True, "fcier", Me)
        _ColSucursal = New ColumnaMatrixSBOEditText(Of String)("ColSucur", True, "sucur", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _ColDocEntry.AsignaBindingDataTable()
        _ColNoOT.AsignaBindingDataTable()
        _ColNoOTS.AsignaBindingDataTable()
        _ColTipoOT.AsignaBindingDataTable()
        _ColNoUnidad.AsignaBindingDataTable()
        _ColPlaca.AsignaBindingDataTable()
        _ColEstado.AsignaBindingDataTable()
        _ColCono.AsignaBindingDataTable()
        _ColVisita.AsignaBindingDataTable()
        _ColCodCliente.AsignaBindingDataTable()
        _ColNombreCliente.AsignaBindingDataTable()
        _ColMarca.AsignaBindingDataTable()
        _ColEstilo.AsignaBindingDataTable()
        _ColAsesor.AsignaBindingDataTable()
        _ColFApertura.AsignaBindingDataTable()
        _ColFProceso.AsignaBindingDataTable()
        _ColFCierre.AsignaBindingDataTable()
        _ColSucursal.AsignaBindingDataTable()
    End Sub

End Class
