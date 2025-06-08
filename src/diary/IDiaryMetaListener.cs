namespace digipet.diary;

public interface IDiaryMetaListener {
  // called on insert or delete
  void OnCharInput();

  // called when we begin saving
  void OnSaveBegin();

  // called when save is complete
  void OnSaveComplete();
}