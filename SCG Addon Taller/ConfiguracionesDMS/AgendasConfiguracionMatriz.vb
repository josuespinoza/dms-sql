Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class AgendasConfiguracionMatriz : Inherits MatrixSBO

    Private _ColDocEntry As ColumnaMatrixSBOEditText(Of String)
    Private _ColAgenda As ColumnaMatrixSBOEditText(Of String)
    Private _ColEstadoLogico As ColumnaMatrixSBOEditText(Of String)
    Private _ColIntervalo As ColumnaMatrixSBOEditText(Of String)
    Private _ColAbreviatura As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodAsesor As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodEncargado As ColumnaMatrixSBOEditText(Of String)
    Private _ColRazonCita As ColumnaMatrixSBOEditText(Of String)
    Private _ColArticulo As ColumnaMatrixSBOEditText(Of String)
    Private _ColVisbleWeb As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantLunes As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantMartes As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantMiercoles As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantJueves As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantViernes As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantSabado As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantDomingo As ColumnaMatrixSBOEditText(Of String)

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#Region "Propiedades"

    Public Property ColDocEntry As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColDocEntry
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColDocEntry = value
        End Set
    End Property

    Public Property ColAgenda As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColAgenda
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColAgenda = value
        End Set
    End Property

    Public Property ColEstadoLogico As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColEstadoLogico
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColEstadoLogico = value
        End Set
    End Property

    Public Property ColIntervalo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColIntervalo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColIntervalo = value
        End Set
    End Property

    Public Property ColAbreviatura As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColAbreviatura
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColAbreviatura = value
        End Set
    End Property

    Public Property ColCodAsesor As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCodAsesor
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCodAsesor = value
        End Set
    End Property

    Public Property ColCodEncargado As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCodEncargado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCodEncargado = value
        End Set
    End Property

    Public Property ColRazonCita As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColRazonCita
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColRazonCita = value
        End Set
    End Property

    Public Property ColArtuculo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColArticulo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColArticulo = value
        End Set
    End Property

    Public Property ColVisbleWeb As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColVisbleWeb
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColVisbleWeb = value
        End Set
    End Property

    Public Property ColCantLunes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantLunes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantLunes = value
        End Set
    End Property

    Public Property ColCantMartes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantMartes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantMartes = value
        End Set
    End Property

    Public Property ColCantMiercoles As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantMiercoles
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantMiercoles = value
        End Set
    End Property

    Public Property ColCantJueves As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantJueves
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantJueves = value
        End Set
    End Property

    Public Property ColCantViernes As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantViernes
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantViernes = value
        End Set
    End Property

    Public Property ColCantSabado As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantSabado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantSabado = value
        End Set
    End Property

    Public Property ColCantDomingo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantDomingo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantDomingo = value
        End Set
    End Property

#End Region


    Public Overrides Sub CreaColumnas()
        _ColDocEntry = New ColumnaMatrixSBOEditText(Of String)("col_ID", True, "DocNum", Me)
        _ColAgenda = New ColumnaMatrixSBOEditText(Of String)("col_Agenda", True, "U_Agenda", Me)
        _ColEstadoLogico = New ColumnaMatrixSBOEditText(Of String)("col_Estado", True, "U_EstadoLogico", Me)
        _ColIntervalo = New ColumnaMatrixSBOEditText(Of String)("col_Intev", True, "U_IntervaloCitas", Me)
        _ColAbreviatura = New ColumnaMatrixSBOEditText(Of String)("col_Abrev", True, "U_Abreviatura", Me)
        _ColCodAsesor = New ColumnaMatrixSBOEditText(Of String)("col_Tecn", True, "U_CodAsesor", Me)
        _ColVisbleWeb = New ColumnaMatrixSBOEditText(Of String)("col_Visib", True, "U_VisibleWeb", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _ColDocEntry.AsignaBindingDataTable()
        _ColAgenda.AsignaBindingDataTable()
        _ColEstadoLogico.AsignaBindingDataTable()
        _ColIntervalo.AsignaBindingDataTable()
        _ColAbreviatura.AsignaBindingDataTable()
        _ColCodAsesor.AsignaBindingDataTable()
        _ColVisbleWeb.AsignaBindingDataTable()
    End Sub
End Class
