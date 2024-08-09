import styles from "./Spinner.module.css";
function Spinner() {
  return (
    <div className={styles.spinnerOverlay}>
      <div className={styles.spinner} />
    </div>
  );
}

export default Spinner;
