Option Strict On
Option Explicit On

Imports SCG.UX.Windows.CitasAutomaticas
Imports System.Data.SqlClient

Namespace ServicioAlCliente.ProgramacionCitas

    Public Class AdministradorFiltrosVehiculos
        Implements IAdministradorFiltros

        Private _dataSet As DataSet
        Private _cadenaConexion As String
        Private _dataAdapter As SqlDataAdapter
        Private Const _nombreTabla As String = "Vehiculos"

        Public Sub New(ByVal cadenaConexion As String)
            _cadenaConexion = cadenaConexion
        End Sub



        Public Sub CargaDataSet(ByVal where As String)
            Dim query As String = "SELECT * FROM [@SCGD_VEHICULO] with(nolock)"
            If String.IsNullOrEmpty(where) Then _
            _dataAdapter = New SqlDataAdapter(query, _cadenaConexion) _
            Else _
            _dataAdapter = New SqlDataAdapter(query + " WHERE " + where, _cadenaConexion)
            _dataSet = New DataSet()
            _dataAdapter.Fill(_dataSet, _nombreTabla)
        End Sub

        Public Function ElementosCitas() As IEnumerable(Of IElementoCita) Implements IAdministradorFiltros.ElementosCitas
            Return ConvertirDataRowsAIElementoCita(_dataSet.Tables(_nombreTabla).Rows)
        End Function

        Public Function ElementosCitas(ByVal filtros As IEnumerable(Of IFiltro)) As IEnumerable(Of IElementoCita) Implements IAdministradorFiltros.ElementosCitas
            Dim filtroDataSet As String = String.Empty
            Dim diccionario As Dictionary(Of Integer, String) = New Dictionary(Of Integer, String)()

            CargaDataSet("1=0")
            Dim tablaVehiculos As DataTable = _dataSet.Tables(_nombreTabla)

            For Each filtro As IFiltro In filtros
                If tablaVehiculos.Columns.Contains(filtro.Condicion.Split(" "c)(0).Trim()) AndAlso filtro.Activo Then
                    If diccionario.ContainsKey(filtro.CodigoCategoriaFiltro) Then
                        diccionario(filtro.CodigoCategoriaFiltro) = diccionario(filtro.CodigoCategoriaFiltro) + " OR " + filtro.Condicion
                    Else
                        diccionario.Add(filtro.CodigoCategoriaFiltro, filtro.Condicion)
                    End If
                End If
            Next

            For Each pair As KeyValuePair(Of Integer, String) In diccionario
                If String.IsNullOrEmpty(filtroDataSet) Then
                    filtroDataSet = String.Format("({0})", pair.Value)
                Else
                    filtroDataSet = filtroDataSet + String.Format(" AND ({0})", pair.Value)
                End If
            Next

            CargaDataSet(filtroDataSet)
            tablaVehiculos = _dataSet.Tables(_nombreTabla)
            tablaVehiculos.Columns.Add("idFiltro", GetType(IFiltro))

            For Each filtro As IFiltro In filtros
                If tablaVehiculos.Columns.Contains(filtro.Condicion.Split(" "c)(0).Trim()) AndAlso filtro.Activo Then
                    Dim rows As DataRow() = tablaVehiculos.Select(filtro.Condicion)
                    For Each row As DataRow In rows
                        row("idFiltro") = filtro
                    Next
                End If
            Next

            Return ConvertirDataRowsAIElementoCita(tablaVehiculos.Rows)

        End Function

        Private Function ConvertirDataRowsAIElementoCita(ByVal dataRows As DataRowCollection) As IEnumerable(Of IElementoCita)
            Dim listaResultado As List(Of IElementoCita) = New List(Of IElementoCita)()
            For Each row As DataRow In dataRows
                Dim elementoCita As Vehiculo
                Dim fechaPS As Nullable(Of DateTime)
                Dim fechaUS As Nullable(Of DateTime)
                Dim frec As Nullable(Of Integer)
                Dim marca As String = String.Empty
                Dim modelo As String = String.Empty
                Dim estilo As String = String.Empty
                Dim codMarca As String = String.Empty
                Dim codModelo As String = String.Empty
                Dim codEstilo As String = String.Empty
                Dim cardCode As String = String.Empty
                Dim numPlaca As String = String.Empty
                Dim vin As String = String.Empty
                Dim codUnidad As String = String.Empty
                Dim cardName As String = String.Empty

                If Not row.IsNull(Vehiculo.ColumnaFechaProximoServicio) Then fechaPS = CType(row(Vehiculo.ColumnaFechaProximoServicio), DateTime)
                If Not row.IsNull(Vehiculo.ColumnaFechaUltimoServicio) Then fechaUS = CType(row(Vehiculo.ColumnaFechaUltimoServicio), DateTime)
                If Not row.IsNull(Vehiculo.ColumnaFrecuenciaServicio) Then frec = CInt(row(Vehiculo.ColumnaFrecuenciaServicio))

                If Not row.IsNull(Vehiculo.ColumnaDescMarca) Then marca = row(Vehiculo.ColumnaDescMarca).ToString()
                If Not row.IsNull(Vehiculo.ColumnaDescEstilo) Then estilo = row(Vehiculo.ColumnaDescEstilo).ToString()
                If Not row.IsNull(Vehiculo.ColumnaDescModelo) Then modelo = row(Vehiculo.ColumnaDescModelo).ToString()
                If Not row.IsNull(Vehiculo.ColumnaCodMarca) Then codMarca = row(Vehiculo.ColumnaCodMarca).ToString()
                If Not row.IsNull(Vehiculo.ColumnaCodEstilo) Then codEstilo = row(Vehiculo.ColumnaCodEstilo).ToString()
                If Not row.IsNull(Vehiculo.ColumnaCodModelo) Then codModelo = row(Vehiculo.ColumnaCodModelo).ToString()
                If Not row.IsNull(Vehiculo.ColumnaCardCode) Then cardCode = row(Vehiculo.ColumnaCardCode).ToString()
                If Not row.IsNull(Vehiculo.ColumnaPlaca) Then numPlaca = row(Vehiculo.ColumnaPlaca).ToString()
                If Not row.IsNull(Vehiculo.ColumnaNoUnidad) Then codUnidad = row(Vehiculo.ColumnaNoUnidad).ToString()
                If Not row.IsNull(Vehiculo.ColumnaCardName) Then cardName = row(Vehiculo.ColumnaCardName).ToString()

                vin = row(Vehiculo.ColumnaVIN).ToString()
                elementoCita = New Vehiculo(row(Vehiculo.ColumnaCode).ToString(), vin, fechaPS, fechaUS, frec)

                elementoCita.DescMarca = marca
                elementoCita.DescEstilo = estilo
                elementoCita.DescModelo = modelo
                elementoCita.CodMarca = codMarca
                elementoCita.CodEstilo = codEstilo
                elementoCita.CodModelo = codModelo
                elementoCita.CardCode = cardCode
                elementoCita.NumPlaca = numPlaca
                elementoCita.Vin = vin
                elementoCita.CodUnidad = codUnidad
                elementoCita.CardName = cardName

                If Not (row.IsNull("idFiltro")) Then elementoCita.Filtro = CType(row("idFiltro"), IFiltro)
                listaResultado.Add(elementoCita)

            Next
            Return listaResultado

        End Function

    End Class

End Namespace