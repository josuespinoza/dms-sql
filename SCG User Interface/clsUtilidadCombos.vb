Namespace SCG_User_Interface

    Public Class clsUtilidadCombos

#Region "SubClases"

        Public Class UI_ItemCombo

#Region "Declaraciones"

            Private strValor As String
            Private strDescripcion As String
            Public Const mc_Descripcion As String = "Descripcion"
            Public Const mc_Valor As String = "Valor"

#End Region

#Region "Constructor"

            Public Sub New(ByVal p_strDescripcion As String, ByVal p_strValor As String)
                MyBase.New()
                Me.strValor = p_strValor
                Me.strDescripcion = p_strDescripcion
            End Sub

#End Region

#Region "Propiedades"

            Public ReadOnly Property Valor() As String
                Get
                    Return strValor
                End Get
            End Property

            Public ReadOnly Property Descripcion() As String
                Get
                    Return strDescripcion
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return Me.strDescripcion & Space(100) & Me.strValor
            End Function

#End Region

        End Class

#End Region

#Region "General"

        Private Shared Sub CargarComboSourceByArrayList(ByRef p_objComboBox As ComboBox, ByRef p_alstItemsCombo As ArrayList)

            If Not IsNothing(CType(p_objComboBox.DataSource, ArrayList)) Then
                CType(p_objComboBox.DataSource, ArrayList).Clear()
            End If

            p_objComboBox.DataSource = Nothing

            If p_alstItemsCombo.Count <> 0 Then


                p_objComboBox.DataSource = p_alstItemsCombo
                p_objComboBox.DisplayMember = UI_ItemCombo.mc_Descripcion
                p_objComboBox.ValueMember = UI_ItemCombo.mc_Valor


            End If

        End Sub

#End Region

#Region "Específicos"

        Shared Sub CargarComboSourceByReader(ByRef p_objComboBox As ComboBox, ByVal p_drdListaDatos As SqlClient.SqlDataReader, _
                   ByVal p_strDescripcion As String, ByVal p_strValor As String)

            Dim alstItemsCombo As New ArrayList

            If Not IsNothing(CType(p_objComboBox.DataSource, ArrayList)) Then
                CType(p_objComboBox.DataSource, ArrayList).Clear()
            End If

            p_objComboBox.DataSource = Nothing

            If Not IsNothing(p_drdListaDatos) Then

                While p_drdListaDatos.Read
                    alstItemsCombo.Add(New clsUtilidadCombos.UI_ItemCombo(p_drdListaDatos.Item(p_strDescripcion), p_drdListaDatos.Item(p_strValor)))
                    Debug.Print(p_drdListaDatos.Item(p_strDescripcion) & "  " & p_drdListaDatos.Item(p_strValor))
                End While

                If alstItemsCombo.Count <> 0 Then

                    p_objComboBox.DataSource = alstItemsCombo
                    p_objComboBox.DisplayMember = UI_ItemCombo.mc_Descripcion
                    p_objComboBox.ValueMember = UI_ItemCombo.mc_Valor

                End If

            End If

        End Sub

        Public Shared Sub ComboTiposItems(ByRef p_cboDestino As ComboBox)
            Dim alstTipos As ArrayList

            alstTipos = New ArrayList

            With alstTipos

                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Refacciones, "1"))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Servicios, "2"))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Suministros, "3"))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.ServiciosExternos, "4"))

            End With

            CargarComboSourceByArrayList(p_cboDestino, alstTipos)

        End Sub

        Public Shared Sub CargarComboEstadoOrdenes(ByRef p_cboDestino As ComboBox, ByVal Todos As Boolean)
            Dim alstTipos As ArrayList

            alstTipos = New ArrayList

            With alstTipos

                .Add(New UI_ItemCombo(My.Resources.ResourceUI.NoIniciada, My.Resources.ResourceUI.NoIniciada))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Enproceso, My.Resources.ResourceUI.Enproceso))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Suspendida, My.Resources.ResourceUI.Suspendida))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Finalizada, My.Resources.ResourceUI.Finalizada))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Cancelada, My.Resources.ResourceUI.Cancelada))
                'verifico qeu se deseen cargar todos los estados en el combo
                If Todos Then
                    .Add(New UI_ItemCombo(My.Resources.ResourceUI.Cerrada, My.Resources.ResourceUI.Cerrada))
                    .Add(New UI_ItemCombo(My.Resources.ResourceUI.Facturada, My.Resources.ResourceUI.Facturada))
                    .Add(New UI_ItemCombo(My.Resources.ResourceUI.Entregada, My.Resources.ResourceUI.Entregada))
                End If
                

            End With

            CargarComboSourceByArrayList(p_cboDestino, alstTipos)

        End Sub

        Public Shared Sub CargarComboEstadoOT(ByRef p_cboDestino As ComboBox)
            Dim alstTipos As ArrayList

            alstTipos = New ArrayList

            With alstTipos
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.NoIniciada, 1))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Enproceso, 2))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Suspendida, 3))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Finalizada, 4))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Cancelada, 5))
            End With

            CargarComboSourceByArrayList(p_cboDestino, alstTipos)

        End Sub


        Public Shared Sub CargarComboEstadoVisitas(ByRef p_cboDestino As ComboBox)
            Dim alstTipos As ArrayList

            alstTipos = New ArrayList

            With alstTipos
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Enproceso, My.Resources.ResourceUI.Enproceso))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Suspendida, My.Resources.ResourceUI.Suspendida))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Finalizada, My.Resources.ResourceUI.Finalizada))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Entregado, My.Resources.ResourceUI.Entregado))
            End With

            CargarComboSourceByArrayList(p_cboDestino, alstTipos)

        End Sub

        Public Shared Sub CargarComboEstadoSolicitudesEspecificas(ByRef p_cboDestino As ComboBox)
            Dim alstTipos As ArrayList

            alstTipos = New ArrayList

            With alstTipos
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.SinResponder, "Sin Respuesta"))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Respondida, "Respondida"))
                .Add(New UI_ItemCombo(My.Resources.ResourceUI.Cancelada, "Cancelada"))

            End With

            CargarComboSourceByArrayList(p_cboDestino, alstTipos)

        End Sub

#End Region

    End Class

End Namespace
