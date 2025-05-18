-- Función que revisa si, tras cambiar un MotoInventory de 'Available',
-- quedan más disponibles para el mismo post; si no, marca el Post como 'SoldOut'.
CREATE OR REPLACE FUNCTION fn_update_post_status_on_motoinv_change()
RETURNS trigger AS $$
BEGIN
  IF (OLD.status = 'Available')
     AND (NEW.status <> 'Available')
     AND (NEW.post_id IS NOT NULL) THEN

    -- Si no quedan motos 'Available' para este post, actualizar Post
    IF NOT EXISTS (
      SELECT 1
      FROM MotoInventory
      WHERE post_id = NEW.post_id
        AND status = 'Available'
    ) THEN
      UPDATE Post
      SET status     = 'SoldOut',
          updated_at = CURRENT_TIMESTAMP
      WHERE id = NEW.post_id;
    END IF;
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger que dispara la función tras cada UPDATE de status
CREATE TRIGGER trg_motoinv_status_after_update
AFTER UPDATE OF status ON MotoInventory
FOR EACH ROW
EXECUTE FUNCTION fn_update_post_status_on_motoinv_change();