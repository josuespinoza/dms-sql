Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizBusquedaCitas : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub


#Region "Declaraciones"

    Private _ColDocCita As ColumnaMatrixSBOEditText(Of String)
    Private _ColCita As ColumnaMatrixSBOEditText(Of String)
    Private _ColDocEntry As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoOT As ColumnaMatrixSBOEditText(Of String)
    Private _ColTipoOT As ColumnaMatrixSBOEditText(Of String)
    Private _ColSucursal As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoUnidad As ColumnaMatrixSBOEditText(Of String)
    Private _ColPlaca As ColumnaMatrixSBOEditText(Of String)
    Private _ColConfirmacion As ColumnaMatrixSBOEditText(Of String)
    Private _ColCono As ColumnaMatrixSBOEditText(Of String)
    Private _ColVisita As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodCliente As ColumnaMatrixSBOEditText(Of String)
    Private _ColNombreCliente As ColumnaMatrixSBOEditText(Of String)
    Private _ColMarca As ColumnaMatrixSBOEditText(Of String)
    Private _ColEstilo As ColumnaMatrixSBOEditText(Of String)
    Private _ColModelo As ColumnaMatrixSBO(Of String)
    Private _ColMecanico As ColumnaMatrixSBOEditText(Of String)
    Private _ColAsesor As ColumnaMatrixSBOEditText(Of String)
    Private _ColFechaCita As ColumnaMatrixSBOEditText(Of String)
    Private _ColHoraCita As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

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

    Public Property ColConfirmacion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColConfirmacion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColConfirmacion = value
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

    Public Property ColModelo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColModelo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColModelo = value
        End Set
    End Property

    Public Property ColMecanico As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColMecanico
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColMecanico = value
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


    Public Property ColVisita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColVisita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColVisita = value
        End Set
    End Property

    Public Property ColCita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCita = value
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

    Public Property ColDocCita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColDocCita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColDocCita = value
        End Set
    End Property

    Public Property ColFechaCita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColFechaCita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColFechaCita = value
        End Set
    End Property

    Public Property ColHoraCita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColHoraCita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColHoraCita = value
        End Set
    End Property

#End Region

    Public Overrides Sub CreaColumnas()

        ColDocCita = New ColumnaMatrixSBOEditText(Of String)("ColDocCit", True, "docCita", Me)
        ColCita = New ColumnaMatrixSBOEditText(Of String)("ColNoCita", True, "cita", Me)
        ColFechaCita = New ColumnaMatrixSBOEditText(Of String)("ColFCita", True, "fcita", Me)
        ColHoraCita = New ColumnaMatrixSBOEditText(Of String)("ColHCita", True, "hcita", Me)
        ColDocEntry = New ColumnaMatrixSBOEditText(Of String)("ColDocE", True, "docentry", Me)
        ColNoOt = New ColumnaMatrixSBOEditText(Of String)("ColNoOT", True, "noot", Me)
        ColTipoOt = New ColumnaMatrixSBOEditText(Of String)("ColTipOT", True, "tipot", Me)
        ColSucursal = New ColumnaMatrixSBOEditText(Of String)("ColSucur", True, "sucur", Me)
        ColNoUnidad = New ColumnaMatrixSBOEditText(Of String)("ColNoUni", True, "nouni", Me)
        ColPlaca = New ColumnaMatrixSBOEditText(Of String)("ColPlaca", True, "placa", Me)
        ColCono = New ColumnaMatrixSBOEditText(Of String)("ColCono", True, "cono", Me)
        ColVisita = New ColumnaMatrixSBOEditText(Of String)("ColVisi", True, "visita", Me)
        ColConfirmacion = New ColumnaMatrixSBOEditText(Of String)("ColConf", True, "conf", Me)
        ColCodCliente = New ColumnaMatrixSBOEditText(Of String)("ColCodCli", True, "codcli", Me)
        ColNombreCliente = New ColumnaMatrixSBOEditText(Of String)("ColNomCl", True, "nomcl", Me)
        ColMarca = New ColumnaMatrixSBOEditText(Of String)("ColMar", True, "mar", Me)
        ColEstilo = New ColumnaMatrixSBOEditText(Of String)("ColEsti", True, "esti", Me)
        ColModelo = New ColumnaMatrixSBOEditText(Of String)("ColModel", True, "mode", Me)
        ColMecanico = New ColumnaMatrixSBOEditText(Of String)("ColMec", True, "mecanic", Me)
        ColAsesor = New ColumnaMatrixSBOEditText(Of String)("ColAse", True, "asesor", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        ColDocCita.AsignaBindingDataTable()
        ColCita.AsignaBindingDataTable()
        ColFechaCita.AsignaBindingDataTable()
        ColHoraCita.AsignaBindingDataTable()
        ColDocEntry.AsignaBindingDataTable()
        ColNoOt.AsignaBindingDataTable()
        ColTipoOt.AsignaBindingDataTable()
        ColSucursal.AsignaBindingDataTable()
        ColNoUnidad.AsignaBindingDataTable()
        ColPlaca.AsignaBindingDataTable()
        ColCono.AsignaBindingDataTable()
        ColVisita.AsignaBindingDataTable()
        ColConfirmacion.AsignaBindingDataTable()
        ColCodCliente.AsignaBindingDataTable()
        ColNombreCliente.AsignaBindingDataTable()
        ColMarca.AsignaBindingDataTable()
        ColEstilo.AsignaBindingDataTable()
        ColModelo.AsignaBindingDataTable()
        ColMecanico.AsignaBindingDataTable()
        ColAsesor.AsignaBindingDataTable()

    End Sub
End Class
