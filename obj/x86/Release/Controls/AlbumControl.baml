   M S B A M L   `   `   `  ����   Movie Cleanup)    #Movie_Cleanup.Controls.AlbumControl   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008       � 	Resources     
ScaleStyle �� 
TargetType �� Property �� Value �� Triggers :� RoutedEvent	 W� To g� Duration �� TargetProperty�� %�  &	        �� " �.���  �@J�RenderTransformOrigin$ 0.5,0.5&���  � z� ��  :� $ 
MouseEnter��.[��� .���� .��W� $	 1.2X�$ 0:0:0.2D�$ RenderTransform.ScaleX�W� $	 1.2X�$ 0:0:0.2D�$ RenderTransform.ScaleY�:� $ 
MouseLeave��.[��� .���� .��W� $	 1.0X�$ 0:0:0.1D�$ RenderTransform.ScaleX�W� $	 1.0X�$ 0:0:0.1D�$ RenderTransform.ScaleY�.��� #��[  	 � RowDefinitions	 �� $	��100 ��� $��20 ��� $��20 �.+��� $��0�����#cccccc
 �� CornerRadius$
 1��
�������$��1,1,1,1q�._��� -   $��imgAlbumArt��$	��110��$	��100��$
��Fill=�$��Stretch=�$��Stretch=����� *
AlbumArt+�� $��1��$
��Left=�$	��110��$��8(�$
��Bold"����� *	AlbumName+�� $��2��$
��Left=�$	��110��$��8(����� *Artist+