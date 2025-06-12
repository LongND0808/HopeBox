<template>
  <div class="chart-container">
    <div class="header">
      <h3>Doanh số các chiến dịch theo tháng</h3>
      <select v-model="selectedYear" @change="updateChart">
        <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
      </select>
    </div>

    <div class="revenue-summary">
      <div class="summary-box">
        <div class="label">Tổng doanh thu năm {{ selectedYear }}</div>
        <div class="value">{{ totalRevenueForYear.toFixed(2) }} triệu VNĐ</div>
      </div>
      <div class="summary-box">
        <div class="label">Tổng doanh thu toàn bộ</div>
        <div class="value">{{ totalRevenueAllYears.toFixed(2) }} triệu VNĐ</div>
      </div>
    </div>


    <canvas ref="revenueChart"></canvas>
  </div>
</template>

<script>
  import axios from 'axios'
  import { Chart, registerables } from 'chart.js'
  Chart.register(...registerables)

  export default {
    name: "RevenueChart",
    data() {
      return {
        selectedYear: new Date().getFullYear(),
        years: [],
        chartInstance: null,
        dataByYear: {},
        totalRevenueForYear: 0,
        totalRevenueAllYears: 0
      }
    },
    mounted() {
      this.fetchRevenueData()
    },
    methods: {
      async fetchRevenueData() {
        try {
          const res = await axios.get('https://localhost:7213/api/Cause/get-cause-revenue', {
            withCredentials: true
          })

          if (res.data?.status === 200) {
            const data = res.data.responseData
            let overallTotal = 0

            data.forEach(entry => {
              const year = entry.year
              const revenue = entry.monthlyRevenue.map(val => val / 1_000_000) // triệu VNĐ
              this.dataByYear[year] = revenue
              overallTotal += revenue.reduce((a, b) => a + b, 0)
            })

            this.years = Object.keys(this.dataByYear).map(Number).sort()
            this.selectedYear = this.years[this.years.length - 1]
            this.totalRevenueAllYears = overallTotal
            this.updateChart()
          }
        } catch (error) {
          console.error('Lỗi khi fetch revenue data:', error)
        }
      },

      renderChart() {
        if (this.chartInstance) {
          this.chartInstance.destroy()
        }

        const ctx = this.$refs.revenueChart.getContext('2d')
        this.chartInstance = new Chart(ctx, {
          type: 'line',
          data: this.getChartData(this.selectedYear),
          options: this.getChartOptions()
        })
      },

      updateChart() {
        const revenueForYear = this.dataByYear[this.selectedYear] || []
        this.totalRevenueForYear = revenueForYear.reduce((a, b) => a + b, 0)
        this.renderChart()
      },

      getChartData(year) {
        return {
          labels: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
          datasets: [{
            label: `Doanh số (${year}) - triệu VNĐ`,
            data: this.dataByYear[year] || [],
            fill: true,
            borderColor: '#fbc658',
            backgroundColor: 'rgba(251, 198, 88, 0.1)',
            tension: 0.4,
            pointBackgroundColor: '#fbc658',
            pointRadius: 4,
            pointHoverRadius: 6
          }]
        }
      },

      getChartOptions() {
        return {
          responsive: true,
          plugins: {
            legend: {
              labels: {
                color: 'white'
              }
            },
            tooltip: {
              enabled: true,
              callbacks: {
                label: function (context) {
                  const value = context.parsed.y
                  const rounded = value.toFixed(2)
                  return `Doanh số: ${rounded} triệu VNĐ`
                }
              }
            }
          },
          scales: {
            x: {
              ticks: { color: 'white' }
            },
            y: {
              ticks: {
                color: 'white',
                callback: function (value) {
                  return value.toFixed(2)
                }
              },
              beginAtZero: true
            }
          }
        }
      }
    }
  }
</script>

<style scoped>
  .chart-container {
    background: #1e1e2f;
    border-radius: 12px;
    padding: 20px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.3);
  }

  .header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
  }

  h3 {
    margin: 0;
    color: #fbc658;
    font-size: 18px;
  }

  select {
    background: #2a2a3d;
    color: white;
    border: none;
    padding: 6px 12px;
    border-radius: 6px;
    font-size: 14px;
  }

  .totals {
    margin-bottom: 10px;
    color: white;
    font-size: 14px;
  }

  .totals p {
    margin: 4px 0;
  }

  .revenue-summary {
    display: flex;
    gap: 20px;
    margin-bottom: 20px;
    flex-wrap: wrap;
  }

  .summary-box {
    flex: 1;
    min-width: 200px;
    background-color: #2a2a3d;
    padding: 15px 20px;
    border-radius: 10px;
    border-left: 4px solid #f6cf82;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
  }

  .summary-box .label {
    font-size: 14px;
    color: #999;
    margin-bottom: 6px;
  }

  .summary-box .value {
    font-size: 18px;
    color: #f6cf82;
    font-weight: bold;
  }
</style>