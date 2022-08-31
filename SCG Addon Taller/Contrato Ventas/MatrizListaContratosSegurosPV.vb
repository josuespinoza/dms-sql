Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizListaContratosSegurosPV : Inherits MatrixSBO

#Region "Declaraciones"
    Private _columnaCol_IDCont As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Reversado As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Unidad As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Familia As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Cliente As ColumnaMatrixSBOEditText(Of String)
#End Region

#Region "Propiedades"



    Public Property ColumnaColIDContrato As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDCont
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDCont = value
        End Set
    End Property

    Public Property ColumnaColReversado As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Reversado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Reversado = value
        End Set
    End Property

    Public Property ColumnaColUnidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Unidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Unidad = value
        End Set
    End Property

    Public Property ColumnaColFamilia As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Familia
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Familia = value
        End Set
    End Property

    Public Property ColumnaColCliente As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cliente
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cliente = value
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
        ColumnaColIDContrato = New ColumnaMatrixSBOEditText(Of String)("colIDCont", True, "colIDCont", Me)
        ColumnaColReversado = New ColumnaMatrixSBOEditText(Of String)("colRever", True, "colRever", Me)
        ColumnaColUnidad = New ColumnaMatrixSBOEditText(Of String)("colUnid", True, "colUnid", Me)
        ColumnaColFamilia = New ColumnaMatrixSBOEditText(Of String)("colMarca", True, "colMarca", Me)
        ColumnaColCliente = New ColumnaMatrixSBOEditText(Of String)("colCliente", True, "colCliente", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaColIDContrato.AsignaBindingDataTable()
        ColumnaColReversado.AsignaBindingDataTable()
        ColumnaColUnidad.AsignaBindingDataTable()
        ColumnaColFamilia.AsignaBindingDataTable()
        ColumnaColCliente.AsignaBindingDataTable()
    End Sub

#End Region
End Class
