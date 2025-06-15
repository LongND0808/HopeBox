<template>
  <div class="dashboard-container">
    <Sidebar />
    <div class="main-content">
      <div class="dashboard-grid">
        <SummaryCard
          title="Tổng chiến dịch"
          icon="fa-hand-holding-heart"
          :value="stats.causes"
          color="#fbc658"
        />
        <SummaryCard
          title="Người dùng"
          icon="fa-users"
          :value="stats.users"
          color="#51cbce"
        />
        <SummaryCard
          title="Tình nguyện viên"
          icon="fa-user-check"
          :value="stats.volunteers"
          color="#6bd098"
        />
        <SummaryCard
          title="Lượt quyên góp"
          icon="fa-donate"
          :value="stats.donations"
          color="#ef8157"
        />
      </div>

      <div class="charts-grid">
        <RevenueChart />
      </div>

      <ManagementFooter />
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Sidebar from '@/components/Sidebar'
import ManagementFooter from '@/components/ManagementFooter'
import SummaryCard from '@/components/SummaryCard'
import RevenueChart from '@/components/RevenueChart'
import { BASE_URL } from '@/utils/constants';

export default {
  components: {
    Sidebar,
    ManagementFooter,
    SummaryCard,
    RevenueChart
  },
  data() {
    return {
      stats: {
        causes: 0,
        users: 0,
        volunteers: 0,
        donations: 0
      }
    }
  },
  mounted() {
    this.fetchData()
  },
  methods: {
    async fetchData() {
      try {
        const causeRes = await axios.get(`${BASE_URL}/api/Cause/get-count`, { withCredentials: true })
        if (causeRes.data?.status === 200) {
          this.stats.causes = causeRes.data.responseData
        }

        const userRes = await axios.get(`${BASE_URL}/api/User/get-count`, { withCredentials: true })
        if (userRes.data?.status === 200) {
          this.stats.users = userRes.data.responseData
        }

        const volunteerRes = await axios.get(`${BASE_URL}/api/Volunteer/get-count`, { withCredentials: true })
        if (volunteerRes.data?.status === 200) {
          this.stats.volunteers = volunteerRes.data.responseData
        }

        const donationRes = await axios.get(`${BASE_URL}/api/Donation/get-count`, { withCredentials: true })
        if (donationRes.data?.status === 200) {
          this.stats.donations = donationRes.data.responseData
        }

      } catch (error) {
        console.error('Lỗi khi fetch dữ liệu dashboard:', error)
      }
    }
  }
}
</script>

<style scoped>
.dashboard-container {
  display: flex;
  flex-direction: row;
  width: 100%;
}

.main-content {
  margin-left: 250px;
  padding: 20px;
  width: 100%;
  background: #111226;
  min-height: 100vh;
  color: white;
  transition: margin-left 0.3s ease;
}

.dashboard-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
  margin-bottom: 40px;
  max-width: 1200px;
}

.charts-grid {
  display: flex;
  flex-direction: column;
  gap: 20px;
  max-width: 1200px;
}

@media (max-width: 1024px) {
  .dashboard-grid {
    grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
    gap: 15px;
  }
  .main-content {
    padding: 15px;
  }
}

@media (max-width: 768px) {
  .dashboard-container {
    flex-direction: column;
  }
  .main-content {
    margin-left: 0;
    padding: 10px;
  }
  .dashboard-grid {
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: 10px;
  }
}
</style>
