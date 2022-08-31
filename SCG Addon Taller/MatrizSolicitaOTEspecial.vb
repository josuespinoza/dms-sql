
'*******************************************
'*Matriz para manejo las Lineas de la oferta de venta
'*******************************************

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSolicitaOTEspecial : Inherits MatrixSBO

#Region "Declaraciones"

    Private _columnaCol_sel As ColumnaMatrixSBOCheckBox(Of String)
    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Name As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Qty As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Curr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Price As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Obs As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_PorcDesc As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IdRepXOrd As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Costo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IndImpuestos As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_CPen As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CSol As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CRec As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPDe As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPTr As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_CPBo As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Compra As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDLineas As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_TipArtSO As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_IDPaqPadre As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_TreeType As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaColSel As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_sel = value
        End Set
    End Property

    Public Property ColumnaColCode As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Code
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Code = value
        End Set
    End Property

    Public Property ColumnaColName As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Name
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Name = value
        End Set
    End Property

    Public Property ColumnaColQuantity As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Qty
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Qty = value
        End Set
    End Property

    Public Property ColumnaColCurrency As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Curr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Curr = value
        End Set
    End Property

    Public Property ColumnaColPrice As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Price
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Price = value
        End Set
    End Property

    Public Property ColumnaColObservations As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Obs
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Obs = value
        End Set
    End Property

    Private Property ColumnaColPorcDesc As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_PorcDesc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_PorcDesc = value
        End Set
    End Property

    Private Property ColumnaColIdRepXOrd As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IdRepXOrd
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IdRepXOrd = value
        End Set
    End Property

    Private Property ColumnaColCosto As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Costo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Costo = value
        End Set
    End Property

    Private Property ColumnaColIndImpuestos As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IndImpuestos
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IndImpuestos = value
        End Set
    End Property

    Public Property ColumnaColCPen() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPen
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPen = value
        End Set
    End Property

    Public Property ColumnaColCSol() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CSol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CSol = value
        End Set
    End Property

    Public Property ColumnaColCRec() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CRec
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CRec = value
        End Set
    End Property

    Public Property ColumnaColCPDe() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPDe
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPDe = value
        End Set
    End Property

    Public Property ColumnaColCPTr() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPTr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPTr = value
        End Set
    End Property

    Public Property ColumnaColCPBo() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CPBo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CPBo = value
        End Set
    End Property

    Private Property ColumnaColCompra() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Compra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Compra = value
        End Set
    End Property

    Private Property ColumnaIDLineas() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDLineas
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDLineas = value
        End Set
    End Property

    Private Property ColumnaTipArtSO() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_TipArtSO
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_TipArtSO = value
        End Set
    End Property


    Private Property ColumnaColIDPaqPadre() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_IDPaqPadre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_IDPaqPadre = value
        End Set
    End Property

    Private Property ColumnaColTreeType() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_TreeType
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_TreeType = value
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
        ColumnaColSel = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "col_Sel", Me)
        ColumnaColCode = New ColumnaMatrixSBOEditText(Of String)("col_Code", True, "col_Code", Me)
        ColumnaColName = New ColumnaMatrixSBOEditText(Of String)("col_Name", True, "col_Name", Me)
        ColumnaColQuantity = New ColumnaMatrixSBOEditText(Of String)("col_Quant", True, "col_Quant", Me)
        ColumnaColCurrency = New ColumnaMatrixSBOEditText(Of String)("col_Curr", True, "col_Curr", Me)
        ColumnaColPrice = New ColumnaMatrixSBOEditText(Of String)("col_Price", True, "col_Price", Me)
        ColumnaColObservations = New ColumnaMatrixSBOEditText(Of String)("col_Obs", True, "col_Obs", Me)
        ColumnaColPorcDesc = New ColumnaMatrixSBOEditText(Of String)("col_PrcDes", True, "col_PrcDes", Me)
        ColumnaColIdRepXOrd = New ColumnaMatrixSBOEditText(Of String)("col_IdRXOr", True, "col_IdRXOr", Me)
        ColumnaColCosto = New ColumnaMatrixSBOEditText(Of String)("col_Costo", True, "col_Costo", Me)
        ColumnaColIndImpuestos = New ColumnaMatrixSBOEditText(Of String)("col_IndImp", True, "col_IndImp", Me)
        ColumnaColCPen = New ColumnaMatrixSBOEditText(Of Decimal)("col_CPend", True, "col_CPend", Me)
        ColumnaColCSol = New ColumnaMatrixSBOEditText(Of Decimal)("col_CSol", True, "col_CSol", Me)
        ColumnaColCRec = New ColumnaMatrixSBOEditText(Of Decimal)("col_CRec", True, "col_CRec", Me)
        ColumnaColCPDe = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenDev", True, "col_PenDev", Me)
        ColumnaColCPTr = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenTra", True, "col_PenTra", Me)
        ColumnaColCPBo = New ColumnaMatrixSBOEditText(Of Decimal)("col_PenBod", True, "col_PenBod", Me)
        ColumnaColCompra = New ColumnaMatrixSBOEditText(Of String)("col_Compra", True, "col_Compra", Me)
        ColumnaIDLineas = New ColumnaMatrixSBOEditText(Of String)("col_IDLine", True, "col_IDLine", Me)
        ColumnaTipArtSO = New ColumnaMatrixSBOEditText(Of String)("col_TipAr", True, "col_TipAr", Me)
        ColumnaColIDPaqPadre = New ColumnaMatrixSBOEditText(Of String)("col_IDPaqP", True, "col_IDPaqP", Me)
        ColumnaColTreeType = New ColumnaMatrixSBOEditText(Of String)("col_TreeT", True, "col_TreeT", Me)

    End Sub

    Public Overrides Sub LigaColumnas()
        ColumnaColSel.AsignaBindingDataTable()
        ColumnaColCode.AsignaBindingDataTable()
        ColumnaColName.AsignaBindingDataTable()
        ColumnaColQuantity.AsignaBindingDataTable()
        ColumnaColCurrency.AsignaBindingDataTable()
        ColumnaColPrice.AsignaBindingDataTable()
        ColumnaColObservations.AsignaBindingDataTable()
        ColumnaColPorcDesc.AsignaBindingDataTable()
        ColumnaColIdRepXOrd.AsignaBindingDataTable()
        ColumnaColCosto.AsignaBindingDataTable()
        ColumnaColIndImpuestos.AsignaBindingDataTable()
        ColumnaColCPen.AsignaBindingDataTable()
        ColumnaColCSol.AsignaBindingDataTable()
        ColumnaColCRec.AsignaBindingDataTable()
        ColumnaColCPDe.AsignaBindingDataTable()
        ColumnaColCPTr.AsignaBindingDataTable()
        ColumnaColCPBo.AsignaBindingDataTable()
        ColumnaColCompra.AsignaBindingDataTable()
        ColumnaIDLineas.AsignaBindingDataTable()
        ColumnaTipArtSO.AsignaBindingDataTable()
        ColumnaColIDPaqPadre.AsignaBindingDataTable()
        ColumnaColTreeType.AsignaBindingDataTable()
    End Sub

#End Region
End Class
