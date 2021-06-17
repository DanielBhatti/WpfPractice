using System.Windows;

namespace Chapters.Chapter06
{
	public interface IDragSourceAdvisor
	{
		UIElement SourceUI { get; set; }

		DragDropEffects SupportedEffects { get; }

		DataObject GetDataObject(UIElement draggedElt);
		void FinishDrag(UIElement draggedElt, DragDropEffects finalEffects);
		bool IsDraggable(UIElement dragElt);
		UIElement GetTopContainer();
	}
}