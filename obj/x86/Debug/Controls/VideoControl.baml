   M S B A M L   `   `   `  ����  Movie Cleanup)    #Movie_Cleanup.Controls.VideoControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5         
ScaleStyle �� 
TargetType �� Property �� Value �� Triggers :� RoutedEvent	 W� To g� Duration �� TargetProperty�� 5	   
   %�  &	        �� 5
      " �6'   .�5      ��  �@J�RenderTransformOrigin$ 0.5,0.5&�6:   �� 5       � z� 5      �� 5      5       5      :� 5      $ 
MouseEnter��6#   .[�5      �� .��5      �� .��5   "   W� $	 1.2X�62   $ 0:0:0.2D�6;   $ RenderTransform.ScaleX�5   *   W� 5   "   $	 1.2X�62   $ 0:0:0.2D�6;   $ RenderTransform.ScaleY�5   *   5      5      5      :� 5      $ 
MouseLeave��6#   .[�5      �� .��5      �� .��5   "   W� $	 1.0X�62   $ 0:0:0.1D�6;   $ RenderTransform.ScaleX�5    *   W� 5!   "   $	 1.0X�62   $ 0:0:0.1D�6;   $ RenderTransform.ScaleY�5"   *   5#      5$      5%      5'      5(      .��5*      � #��[  6   	 � RowDefinitions	 5+   
   �� 5,      $	��100 �6   6+   �� 5-      $��20 �6   6*   �� 5.      $��20 �6   6*   .+�50   
   �� $��0��6   ���#cccccc
 �� CornerRadius$
 1��66   
�������$��1,1,1,1q�6Z   ._�51      �� -   5        $��imgVideoArt��51      $	��110��6D   $	��100��6P   $
��Fill=�6]   $��Stretch=�6l   $��Stretch=�6�   ��6(   �� *
VideoArt+6�   52      �� 53   
   -   5        $��lblVideoName��53      $��1��6   $
��Left=�6O   $	��110��6j   $��8(�6v   $
��Bold"�6�   ��61   �� *	VideoName+6�   �� 54   
   $��2��6   $
��Left=�66   $	��110��6Q   $��8(�6]   ��6   �� *Year+6l   55      56      